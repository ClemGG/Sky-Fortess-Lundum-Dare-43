using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Killzone : MonoBehaviour {

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Player")
        {
            StatsSystem ss = c.gameObject.GetComponent<StatsSystem>();
            if (ss)
            {
                ss.KillPlayer(0);
                return;
            }
        }

        if (c.gameObject.tag == "WeaponObject")
        {
            c.gameObject.SetActive(false);
        }
    }
}
