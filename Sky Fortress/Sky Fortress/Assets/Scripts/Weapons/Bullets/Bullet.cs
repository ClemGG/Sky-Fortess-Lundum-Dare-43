using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Bullet : MonoBehaviour {



    [Header("Scripts & Components : ")]
    [Space(10)]

    protected Transform t;
    protected Rigidbody rb;
    protected Quaternion originalRotation;

    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [HideInInspector] public int bulletID;
    protected Vector3 startPos;
    [SerializeField] protected int dmgToDeal;
    [SerializeField] protected float maxRange;
    [SerializeField] protected float forceToApply;

    [Space(10)]

    [SerializeField] protected string[] prefabsToSpawnOnDeath;


    private int i = 0;

    private void OnEnable()
    {

        startPos = transform.position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        originalRotation = transform.rotation;

        Impulse();
    }


    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        startPos = t.position;

    }


    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void SpawnPrefabsOnDeath()
    {
        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {

            GameObject go = ObjectPooler.instance.SpawnFromPool(prefabsToSpawnOnDeath[i], t.position, ObjectPooler.instance.GetTransformFromTag(prefabsToSpawnOnDeath[i]).rotation);

            if (go.GetComponent<ExplosionAOE>())
            {
                go.GetComponent<ExplosionAOE>().explosionID = bulletID;
            }

            if (go.GetComponent<FlaqueAcid>())
            {
                go.GetComponent<FlaqueAcid>().ID = bulletID;
            }

        }
    }

    protected abstract void Move();


    protected virtual void Impulse()
    {
        
    }

    protected abstract void OnCollisionEnter(Collision col);
}
