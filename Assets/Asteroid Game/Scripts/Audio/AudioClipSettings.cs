using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioClipSettings
{
    public bool music;
    public enum PlayAudioType { oneShot, random, sequence };
    public PlayAudioType playAudioType;
    public int sequenceIndex;
    public bool loop, oneShot = false;
    public AudioClip currentClip;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0, 1)]
    public float volume = 1.0f;
    [Range(-3, 3)]
    public float pitch = 1.0f;
    [Range(-1, 1)]
    public float stereoLeftRightPan = 0.0f;

    public AudioClipSettings(AudioClip audioClip, bool loop, bool oneShot, PlayAudioType playAudioType)
    {
        clips = new List<AudioClip>
        {
            audioClip
        };
        this.loop = loop;
        this.oneShot = oneShot;
        this.playAudioType = playAudioType;
    }

    public void Play()
    {
        AudioManager.Instance.Play(this, music);
    }

    public void Stop()
    {
        AudioManager.Instance.Stop(this, music);
    }
}