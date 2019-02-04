using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public enum WeaponType { Red,Green,Yellow,Blue};
    public WeaponType weaponType;


    [Space(10)]

    [SerializeField] protected AudioSource shotAud;

    [Space(10)]

    public bool isAnUpgrade;
    public float currentExp = 0f;
    public float upgradeExp = 5f;

    public int weaponID = 0;

    [Space(10)]

    public float fireRate;
    [HideInInspector] public float timer;
    public Sprite weaponLogo;
    public Color logoColor;
    [HideInInspector] public Transform firePoint;
    public string bulletPrefabTag;
    
    [Space(10)]

    public Sprite logoUpgrade;
    public float fireRateUpgrade;
    public string bulletUpgradeTag;


    [HideInInspector] public Weapon thisWeapon;

    protected virtual void Start()
    {
        timer = fireRate;

        if(firePoint == null)
            firePoint = transform.GetChild(0);

        thisWeapon = this;

        shotAud = GetComponent<AudioSource>();
    }


    public void GainExp()
    {
        if (isAnUpgrade)
            return;

        currentExp += 1f;
        if(currentExp >= upgradeExp)
        {
            isAnUpgrade = true;
            OnWeaponUpgrade();
            bulletPrefabTag = bulletUpgradeTag;
            fireRate = fireRateUpgrade;
            timer = fireRate;
            weaponLogo = logoUpgrade;
            
        }


    }

    public virtual void NewWeapon(Weapon lastWeapon)
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

            this.fireRate = fireRateUpgrade;
            this.weaponLogo = logoUpgrade;
            this.bulletPrefabTag = bulletUpgradeTag;
        }
        

        this.timer = fireRate;
        //this.firePoint = transform.GetChild(0);

        thisWeapon = this;

    }



    protected virtual void OnWeaponUpgrade()
    {
        
    }

    public abstract void Shoot();
    public abstract void ReleaseBullet();
}
