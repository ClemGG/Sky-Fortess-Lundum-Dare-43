using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour {

    public string weaponPrefabTag;
    public Transform spawnPoint;
    public GameObject spawnedWeapon;
    public float spawnInterval = 30f;
    private float timer;
    public bool isFree = true;


    private void Start()
    {
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update () {

        if (spawnedWeapon != null)
        {
            //Si un joueur a ramassé l'arme, alors on peut reprendre le countdown
            if (spawnedWeapon.transform.parent != spawnPoint)
            {
                isFree = true;
            }
        }
        
        //Tant qu'un joueur n'a pas ramassé l'arme, on n'en crée pas de nouvelles
        if (!isFree)
            return;

		if(timer < spawnInterval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ReleaseNewWeapon();
            timer = 0f;
        }
    }

    private void ReleaseNewWeapon()
    {
        spawnedWeapon = ObjectPooler.instance.SpawnFromPool(weaponPrefabTag, spawnPoint.position, Quaternion.identity);
        spawnedWeapon.transform.parent = spawnPoint;
        isFree = false;
    }
}
