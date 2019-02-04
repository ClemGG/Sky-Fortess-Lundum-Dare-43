using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBullet : MonoBehaviour {


    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [HideInInspector] public int bulletID;
    [SerializeField] protected int dmgToDeal;



    private void OnParticleCollision(GameObject c)
    {
        MovementSystem ms = c.GetComponent<MovementSystem>();

        if(ms && ms.joueurID != bulletID)
        {
            ms.stats.TakeDmg(dmgToDeal, bulletID);
        }
    }
}
