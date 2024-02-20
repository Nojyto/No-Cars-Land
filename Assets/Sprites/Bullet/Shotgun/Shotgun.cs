using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField]
    private float numberOfBullets = 5f;

    [SerializeField]
    private float spreadAngle = 90f;
    private float cooldown = 4f;
    public float damage = 4f;

    public void ShootWeapon(Transform bulletPos, GameObject bulletPrefab)
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calculate a random spread angle for each bullet
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);

            // Create a new rotation based on the bulletPos rotation and the random spread
            Quaternion randomRotation = bulletPos.rotation * Quaternion.Euler(0, 0, randomSpread);

            // Instantiate the bullet with the calculated rotation
            GameObject clone = Instantiate(bulletPrefab, bulletPos.position, randomRotation);

            // Optionally add additional random rotation
            clone.transform.Rotate(Vector3.forward, Random.Range(-10, 10));

            clone.GetComponent<EnemyBulletShotgun>().SetDamage(damage);
        }
    }

    public float getCooldownForWeapon()
    {
        return cooldown;
    }
}
