using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour, IWeapon
{
    [SerializeField]
    private float cooldown = 1f;
    public float damage = 2f;

    public void ShootWeapon(Transform bulletPos, GameObject bulletPrefab)
    {
        GameObject clone = Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);
        clone.GetComponent<EnemyBullet>().SetDamage(damage);
    }

    public float getCooldownForWeapon()
    {
        return cooldown;
    }
}
