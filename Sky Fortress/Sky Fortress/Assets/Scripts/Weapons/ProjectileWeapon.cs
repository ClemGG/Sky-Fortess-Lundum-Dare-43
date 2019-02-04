using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {


    public override void NewWeapon(Weapon lastWeapon)
    {
        this.weaponType = lastWeapon.weaponType;

        this.isAnUpgrade = lastWeapon.isAnUpgrade;
        this.currentExp = lastWeapon.currentExp;
        this.logoColor = lastWeapon.logoColor;

        if (!isAnUpgrade)
        {

            this.fireRate = lastWeapon.fireRate;
            this.weaponLogo = lastWeapon.weaponLogo;
            this.bulletPrefabTag = lastWeapon.bulletPrefabTag;
        }
        else
        {

            this.fireRate = lastWeapon.fireRateUpgrade;
            this.weaponLogo = lastWeapon.logoUpgrade;
            this.bulletPrefabTag = lastWeapon.bulletUpgradeTag;
        }


        this.timer = lastWeapon.fireRate;
        this.firePoint = lastWeapon.transform.GetChild(0);

        thisWeapon = this;
    }




    public override void Shoot()
    {
            //GameObject go = ObjectPooler.instance.SpawnFromPool(bulletPrefabTag, firePoint.position, firePoint.rotation);
            GameObject go = ObjectPooler.instance.SpawnFromPool(bulletPrefabTag, firePoint.position, firePoint.rotation);
            go.GetComponent<Bullet>().bulletID = weaponID;
            timer = 0f;
            shotAud.Play();
    }


    public override void ReleaseBullet()
    {
        //Rien à faire ici ; c'est pour le script ContinousWeapon, qui doit libérer son système de particules / laser quand le joueur relâche la souris
    }
}
