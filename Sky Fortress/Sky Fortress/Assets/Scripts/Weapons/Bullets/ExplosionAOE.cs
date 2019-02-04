using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAOE : MonoBehaviour {

    [HideInInspector] public int explosionID = 0;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {

            MovementSystem ms = c.gameObject.GetComponent<MovementSystem>();

            if (ms.joueurID != explosionID)
            {
                ms.gameObject.GetComponent<StatsSystem>().KillPlayer(explosionID);
            }

        }
    }

}
