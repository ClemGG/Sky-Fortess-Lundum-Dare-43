using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : Bullet {

    
    protected override void Move()
    {
        if (Vector3.Distance(t.position, startPos) > maxRange)
        {
            gameObject.SetActive(false);
            return;
        }

    }

    protected override void Impulse()
    {
        Vector3 direction = new Vector3(0f, .4f, 1f).normalized * forceToApply;
        GetComponent<Rigidbody>().AddRelativeForce(direction, ForceMode.Impulse);



    }


    protected override void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            MovementSystem ms = col.gameObject.GetComponent<MovementSystem>();

            if (ms.joueurID != bulletID)
            {
                SpawnPrefabsOnDeath();
                gameObject.SetActive(false);
            }
        }
        else
        {

            SpawnPrefabsOnDeath();
            gameObject.SetActive(false);
        }

        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }


}

