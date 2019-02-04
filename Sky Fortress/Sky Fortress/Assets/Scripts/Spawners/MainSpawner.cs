using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour {

    public Transform[] spawnersOfThisObject;

    [SerializeField] private string greenBulletTag;

    private int i = 0;



    private void OnEnable()
    {
        //Pour éviter que les spawners ne s'activent dès la début
        if (i > 0)
        {
            StartCoroutine(StartSwpawners());
        }

        i++;
    }

    private IEnumerator StartSwpawners()
    {
        for (int i = 0; i < spawnersOfThisObject.Length; i++)
        {
            SpawnAdditionallGreenBullet(i);
        }
        yield return 0;
    }

    public void SpawnAdditionallGreenBullet(int i)
    {
        
        //La force de poussée est ajoutée à la balle depuis son propre script, grâce à IObjectPooler
        ObjectPooler.instance.SpawnFromPool(greenBulletTag, spawnersOfThisObject[i].position, spawnersOfThisObject[i].rotation);
        
    }
}
