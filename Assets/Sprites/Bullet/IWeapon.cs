using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void ShootWeapon(Transform bulletPos, GameObject bulletPrefab);
    float getCooldownForWeapon();
}
