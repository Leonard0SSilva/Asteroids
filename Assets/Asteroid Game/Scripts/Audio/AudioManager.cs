using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static AudioClipSettings;

// Manages audio settings, music, and sound effects for the game.
// This class is responsible for configuring and controlling audio settings, including music and sound effects playback.
// It also handles the management of multiple audio sources for sound effects.
// The class uses Unity's Audio Mixer for controlling audio volumes and provides functions
// to play, stop, and control audio playback based on various settings.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public FloatReference fadeSpeed, musicVolume, sfxVolume;
    public AudioMixerGroup musicMixer, sfxMixer;


    [SerializeField]
    private AudioSource musicSource, oneShotSFXSource;

    private readonly List<AudioSource> sfxSources = new();

    [SerializeField]
    private GameObject sfxSourceGO;

    private Coroutine SwitchAndFadeMusicRoutine;
    private Coroutine FadeOutMusicRoutine;

    public AudioClip CurrentMusic
    {
        get { return musicSource.clip; }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.AddComponent<DontDestroyOnLoad>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        var listener = new GameObject("Audio Listener");
        listener.AddComponent<AudioListener>();
        listener.transform.SetParent(gameObject.transform);

        var child = new GameObject("Music Source");
        child.transform.SetParent(gameObject.transform);
        musicSource = child.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixer;

        sfxSourceGO = new GameObject("SFX Source");
        sfxSourceGO.transform.SetParent(gameObject.transform);
        oneShotSFXSource = sfxSourceGO.AddComponent<AudioSource>();
        oneShotSFXSource.playOnAwake = false;
        oneShotSFXSource.outputAudioMixerGroup = sfxMixer;
        oneShotSFXSource.volume = 1;
        oneShotSFXSource.enabled = true;

        UpdateVolumes();
    }

    private void Play(AudioClip clip, AudioSource source, float volume = 1f,
                       bool looped = false, float pitch = 1, float panStereo = 0)
    {
        source.volume = volume;
        source.loop = looped;
        source.pitch = pitch;
        source.panStereo = panStereo;
        source.clip = clip;
        source.Play();
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        while (!Mathf.Approximately(source.volume, 0))
        {
            source.volume = Mathf.MoveTowards(source.volume, 0, Time.deltaTime * fadeSpeed.Value);

            yield return new WaitForEndOfFrame();
        }

        source.volume = 0;
        source.Stop();
    }

    #region SFX
    private AudioSource GetAvailableSfxSource(AudioClipSettings audioClipSettings)
    {
        AudioSource availableAudioSource = null;
        bool availableAudioSourceFound = false;

        for (int i = 0; i < sfxSources.Count; i++)
        {
            AudioSource source = sfxSources[i];

            if (!source.isPlaying)
            {
                if (!availableAudioSourceFound)
                {
                    availableAudioSourceFound = true;
                    availableAudioSource = source;
                }

                continue;
            }

            if (source.clip == audioClipSettings.currentClip &&
                 Math.Abs(source.volume - audioClipSettings.volume) < 0.01f)
            {
                return null;
            }
        }

        if (availableAudioSourceFound)
        {
            return availableAudioSource;
        }

        AudioSource audioSource = sfxSourceGO.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = sfxMixer;
        sfxSources.Add(audioSource);

        return audioSource;
    }

    private void UpdateVolume(AudioMixerGroup mixerGroup, float value)
    {
        mixerGroup.audioMixer.SetFloat("volume", value);
    }

    public void UpdateMusicVolume()
    {
        UpdateVolume(musicMixer, musicVolume.Value);
    }

    public void UpdateSFXVolume()
    {
        UpdateVolume(sfxMixer, sfxVolume.Value);
    }

    public void UpdateVolumes()
    {
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    public AudioSource PlaySfx(AudioClipSettings audioClipSettings, bool isOneShot)
    {
        AudioSource source = null;
        if (isOneShot)
        {
            PlaySfx(audioClipSettings, true, oneShotSFXSource);
        }
        else
        {
            source = GetAvailableSfxSource(audioClipSettings);
            PlaySfx(audioClipSettings, false, source);
        }
        return source;
    }

    public void PlaySfx(AudioClipSettings audioClipSettings, bool isOneShot, AudioSource source)
    {
        if (audioClipSettings == null)
        {
            return;
        }

        if (audioClipSettings.currentClip == null)
        {
            return;
        }

        if (isOneShot)
        {
            if (!source)
            {
                source = oneShotSFXSource;
            }

            source.enabled = true;
            source.PlayOneShot(audioClipSettings.currentClip, audioClipSettings.volume * source.volume);
        }
        else
        {
            if (!source)
            {
                source = GetAvailableSfxSource(audioClipSettings);
            }

            if (source)
            {
                Play(audioClipSettings.currentClip, source, audioClipSettings.volume, audioClipSettings.loop,
                    audioClipSettings.pitch, audioClipSettings.stereoLeftRightPan);
            }
        }
    }

    public bool IsPlayingSfx(AudioClipSettings audioClipSettings)
    {
        if (audioClipSettings == null)
        {
            return false;
        }

        for (int i = 0; i < sfxSources.Count; i++)
        {
            AudioSource item = sfxSources[i];

            if (item.isPlaying && item.clip == audioClipSettings.currentClip)
            {
                return true;
            }
        }

        return false;
    }

    public void StopSfx(AudioClipSettings audioClipSettings)
    {
        if (audioClipSettings == null)
        {
            return;
        }

        for (int i = 0; i < sfxSources.Count; i++)
        {
            AudioSource item = sfxSources[i];

            if (item.clip == audioClipSettings.currentClip)
            {
                item.Stop();
            }
        }
    }

    public void StopAllSfXs()
    {
        oneShotSFXSource.Stop();

        for (int i = 0; i < sfxSources.Count; i++)
        {
            sfxSources[i].Stop();
        }
    }

    #endregion

    #region Music
    public void PlayMusic(AudioClipSettings audioClipSettings)
    {
        StopMusicRoutines();

        if (musicSource.clip == null)
        {
            musicSource.volume = audioClipSettings.volume;
            musicSource.loop = audioClipSettings.loop;
            musicSource.pitch = audioClipSettings.pitch;
            musicSource.panStereo = audioClipSettings.stereoLeftRightPan;
            musicSource.clip = audioClipSettings.currentClip;
            musicSource.Play();
        }
        else
        {
            SwitchAndFadeMusicRoutine = StartCoroutine(SwitchAndFadeMusic(audioClipSettings.currentClip,
                audioClipSettings.volume, audioClipSettings.loop, audioClipSettings.pitch,
                audioClipSettings.stereoLeftRightPan));
        }
    }

    public void StopMusic()
    {
        StopMusicRoutines();
        FadeOutMusicRoutine = StartCoroutine(FadeOutMusic());
    }

    private void StopMusicRoutines()
    {
        if (FadeOutMusicRoutine != null)
        {
            StopCoroutine(FadeOutMusicRoutine);
        }

        if (SwitchAndFadeMusicRoutine != null)
        {
            StopCoroutine(SwitchAndFadeMusicRoutine);
        }
    }

    private IEnumerator SwitchAndFadeMusic(AudioClip clip, float volume = 1f
        , bool looped = false, float pitch = 1, float panStereo = 0)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        if (musicSource.isPlaying)
        {
            while (!Mathf.Approximately(musicSource.volume, 0))
            {
                musicSource.volume = Mathf.MoveTowards(musicSource.volume, 0, Time.deltaTime * fadeSpeed.Value);
                yield return waitForEndOfFrame;
            }
        }

        musicSource.loop = looped;
        musicSource.pitch = pitch;
        musicSource.panStereo = panStereo;
        musicSource.clip = clip;
        musicSource.Play();

        while (!Mathf.Approximately(musicSource.volume, volume))
        {
            musicSource.volume = Mathf.MoveTowards(musicSource.volume, volume, Time.deltaTime * fadeSpeed.Value);

            yield return waitForEndOfFrame;
        }

        musicSource.volume = volume;
    }

    private IEnumerator FadeOutMusic()
    {
        yield return FadeOut(musicSource);
    }
    #endregion

    private void PlayClip(AudioClipSettings audioClipSettings, bool music)
    {
        if (music)
        {
            PlayMusic(audioClipSettings);
        }
        else
        {
            PlaySfx(audioClipSettings, audioClipSettings.oneShot);
        }
    }

    public void Play(AudioClipSettings audioClipSettings, bool music)
    {
        switch (audioClipSettings.playAudioType)
        {
            case PlayAudioType.oneShot:
                foreach (var item in audioClipSettings.clips)
                {
                    audioClipSettings.currentClip = item;
                    PlayClip(audioClipSettings, music);
                }
                break;
            case PlayAudioType.random:

                audioClipSettings.currentClip = audioClipSettings.clips[UnityEngine.Random.Range(0, audioClipSettings.clips.Count)];
                Play(audioClipSettings, music);
                break;
            case PlayAudioType.sequence:
                if (audioClipSettings.sequenceIndex > audioClipSettings.clips.Count)
                {
                    audioClipSettings.sequenceIndex = 0;
                }
                audioClipSettings.currentClip = audioClipSettings.clips[audioClipSettings.sequenceIndex];
                Play(audioClipSettings, music);
                audioClipSettings.sequenceIndex++;
                break;
        }
    }

    public void Stop(AudioClipSettings audioClipSettings, bool music)
    {
        if (music)
        {
            StopMusic();
        }
        else
        {
            StopSfx(audioClipSettings);
        }
    }
}