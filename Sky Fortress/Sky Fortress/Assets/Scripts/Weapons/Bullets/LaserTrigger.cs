using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour {

    private Transform t;
    private BlueLaser blueLaserScript;

    private Coroutine co;


	// Use this for initialization
	void Start () {
        t = transform;
        blueLaserScript = t.parent.GetComponent<BlueLaser>();
	}

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            MovementSystem ms = c.gameObject.GetComponent<MovementSystem>();

            if (ms.joueurID != blueLaserScript.bulletID && co == null)
            {

                co = StartCoroutine(DealLaserDmg(ms));
            }
        }
    }


    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player" && co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    private IEnumerator DealLaserDmg(MovementSystem ms)
    {
        float timer = 0f;

        while (true)
        {
            if(timer < blueLaserScript.laserDelay)
            {
                timer += Time.deltaTime;
            }
            else
            {
                ms.stats.TakeDmg(blueLaserScript.dmgToDeal, blueLaserScript.bulletID);
                timer = 0f;
            }

            yield return 0;
        }

    }
}
