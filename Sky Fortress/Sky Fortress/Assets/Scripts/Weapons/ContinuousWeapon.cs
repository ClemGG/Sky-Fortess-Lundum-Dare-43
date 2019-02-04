using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousWeapon : Weapon {



    [Space(10)]

    [SerializeField] private AudioClip clipUpgrade;
    protected GameObject instantiatedPrefab;


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

        if (instantiatedPrefab == null)
        {
            shotAud.Play();

            instantiatedPrefab = ObjectPooler.instance.SpawnFromPool(bulletPrefabTag, firePoint.position, firePoint.rotation);
            timer = 0f;

            instantiatedPrefab.transform.parent = firePoint;

            BlueBullet bb = instantiatedPrefab.GetComponent<BlueBullet>();
            BlueLaser bl = instantiatedPrefab.GetComponent<BlueLaser>();

            if (bb)
            {
                bb.bulletID = weaponID;
            }
            else if (bl)
            {
                bl.bulletID = weaponID;
            }
        }
    }


    public override void ReleaseBullet()
    {
        shotAud.Stop();


        if (instantiatedPrefab != null)
        {
            instantiatedPrefab.transform.parent = ObjectPooler.instance.transform;
            instantiatedPrefab.SetActive(false);
            instantiatedPrefab = null;
        }
    }


    protected override void OnWeaponUpgrade()
    {
        shotAud.Stop();

        if(clipUpgrade != null)
        {
            shotAud.clip = clipUpgrade;
        }


        if (instantiatedPrefab != null)
        {
            instantiatedPrefab.transform.parent = ObjectPooler.instance.transform;
            instantiatedPrefab.SetActive(false);
            instantiatedPrefab = null;
        }
    }
}
