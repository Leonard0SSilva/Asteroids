using System;
using System.Collections;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public ObjectPool bulletPool;
        public Vector3Reference direction;
        public Transform firePosition;
        public float fireInterval = 2.0f;
        public BoolReference canShoot, shoot;
    }

    public Settings settings;

    private void OnEnable()
    {
        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            if (settings != null)
            {
                if (gameObject.activeSelf && settings.shoot.Value && settings.canShoot.Value)
                {
                    ShootBullet();
                }
                yield return new WaitForSeconds(settings.fireInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = settings.bulletPool.GetObjectFromPool();
        bullet.transform.SetParent(null);

        Vector2 normalizedDirection = settings.direction.Value.normalized;
        float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        bullet.transform.SetPositionAndRotation(settings.firePosition.position, bulletRotation);
    }
}