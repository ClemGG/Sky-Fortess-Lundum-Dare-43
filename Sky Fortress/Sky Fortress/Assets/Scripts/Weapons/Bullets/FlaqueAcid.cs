using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaqueAcid : MonoBehaviour {


    [HideInInspector] public int ID = 0;

    [SerializeField] private float acidDuration = 5f;
    [SerializeField] private int acidDmg;

    [Space(10)]
    
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float flaqueDuration;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;

        CheckIfIsGrounded();
    }

    private void CheckIfIsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit,  10f, whatIsGround))
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    // Update is called once per frame
    void Update () {
		
        if(timer < flaqueDuration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GetComponent<Animator>().SetTrigger("stopFlaque");
        }
	}


    private void OnTriggerStay(Collider c)
    {
        if(c.gameObject.tag == "Player")
        {
            MovementSystem ms = c.gameObject.GetComponent<MovementSystem>();

            if (ms.joueurID != ID && !ms.stats.isOnAcid)
            {
                ms.stats.StartCoroutine(ms.stats.ApplyAcidDmg(ID, acidDuration, acidDmg));
            }
        }
    }


}
