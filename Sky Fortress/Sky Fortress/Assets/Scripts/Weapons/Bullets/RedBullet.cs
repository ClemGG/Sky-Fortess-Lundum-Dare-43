using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBullet : Bullet {


    protected override void Move()
    {
        if(Vector3.Distance(t.position, startPos) > maxRange)
        {
            SpawnPrefabsOnDeath();
            gameObject.SetActive(false);
            return;
        }


        Vector3 direction = t.forward * forceToApply;
        rb.velocity = direction;
    }

    protected override void FixedUpdate()
    {
        t.rotation = originalRotation;
        base.FixedUpdate();
    }


    protected override void OnCollisionEnter(Collision col)
    {
        StatsSystem ss = col.gameObject.GetComponent<StatsSystem>();

        if (ss)
        {
            ss.KillPlayer(bulletID);
        }

        SpawnPrefabsOnDeath();
        gameObject.SetActive(false);
    }


}
