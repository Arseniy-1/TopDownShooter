using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private PistolBulletSpawner _pistolBulletSpawner;

    public override void Shoot()
    {
        if (IsReloaded)
        {
            //GunAnimator.SetTrigger();
            IsReloaded = false;
            _pistolBulletSpawner.Spawn(ShootPoint);
        }
    }
}
