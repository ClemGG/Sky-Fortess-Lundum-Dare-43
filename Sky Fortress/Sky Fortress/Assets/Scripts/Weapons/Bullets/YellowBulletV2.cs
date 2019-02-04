using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBulletV2 : Bullet {

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxDetectionRange;
    private Transform target;
    private StatsSystem[] nearbyPlayers;


    protected override void Move()
    {
        if (Vector3.Distance(t.position, startPos) > maxRange)
        {
            SpawnPrefabsOnDeath();
            gameObject.SetActive(false);
            return;
        }




        target = null;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < nearbyPlayers.Length; i++)
        {

            float f = Vector3.Distance(transform.position, nearbyPlayers[i].transform.position);
            if (f < shortestDistance && f < maxDetectionRange && nearbyPlayers[i].ms.joueurID != bulletID && !nearbyPlayers[i].isDead)
            {
                shortestDistance = f;
                target = nearbyPlayers[i].transform;
                //print(target.name);
            }
        }

        //print(nearbyPlayers.Length);


        if (target)
        {

            Vector3 targetDir = (target.position - transform.position);
            float distanceThisFrame = forceToApply * Time.deltaTime;
            //transform.Translate(targetDir.normalized * distanceThisFrame, Space.World);

            Quaternion lookDir = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0f));

            transform.rotation = lookDir;
        }


        Vector3 direction = t.forward * forceToApply;
        rb.velocity = direction;
    }



    protected override void Impulse()
    {
        nearbyPlayers = FindObjectsOfType<StatsSystem>();
    }





    protected override void FixedUpdate()
    {
        if (!target)
        {
            t.rotation = originalRotation;
        }
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






    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, maxDetectionRange);
    }
}
