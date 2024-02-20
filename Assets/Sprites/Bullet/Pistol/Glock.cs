using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour, IWeapon
{
    private float cooldown = 2f;
    public float damage = 1f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

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
