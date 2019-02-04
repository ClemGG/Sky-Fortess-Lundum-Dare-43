using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMovement : MovementSystem {

    [Space(10)]
    [Header("IA : ")]
    [Space(10)]

    [SerializeField] private MovementSystem target;
    [SerializeField] private WeaponObject targetedWeapon;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsWeapon;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float maxDstToPlayer;
    [SerializeField] private float minDstToPlayer;



    protected override void Start()
    {
        base.Start();
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!GetComponent<StatsSystem>().isDead && canMove)
        {
            DetectEnemyNearby();
        }
    }

    private void DetectEnemyNearby()
    {
        Collider[] cols = Physics.OverlapSphere(t.position, detectionRadius, whatIsPlayer);
        if (cols.Length > 0)
        {
            MovementSystem playerHit = null;
            float shortestDist = Mathf.Infinity;

            for (int i = 0; i < cols.Length; i++)
            {
                playerHit = cols[i].GetComponent<MovementSystem>();

                if (playerHit.joueurID != joueurID && !playerHit.GetComponent<StatsSystem>().isDead)
                {
                    float f = Vector3.Distance(t.position, playerHit.transform.position);

                    if(f < shortestDist)
                    {
                        shortestDist = f;
                        target = playerHit;
                    }
                }
            }
        }
        else
        {
            Collider[] cols2 = Physics.OverlapSphere(t.position, detectionRadius, whatIsWeapon);
            if (cols2.Length > 0)
            {
                WeaponObject weaponHit = null;
                float shortestDist = Mathf.Infinity;

                for (int i = 0; i < cols.Length; i++)
                {
                    weaponHit = cols[i].GetComponent<WeaponObject>();

                    if (weaponHit.transform.GetComponentInChildren(typeof(ParticleSystem)))
                    {
                        float f = Vector3.Distance(t.position, weaponHit.transform.position);

                        if (f < shortestDist)
                        {
                            shortestDist = f;
                            targetedWeapon = weaponHit;
                        }
                    }
                }
            }
            else
            {
                targetedWeapon = null;
            }
        }
    }











    protected override void GetInput()
    {
        if (target)
        {
            Vector3 dir = (target.transform.position - t.position).normalized;


            if(Vector3.Distance(t.position, target.transform.position) > maxDstToPlayer)
            {
                horizontalInput = dir.x;
                verticalInput = dir.z;
            }
            else if (Vector3.Distance(t.position, target.transform.position) < minDstToPlayer)
            {

                horizontalInput = -dir.x;
                verticalInput = -dir.z;
            }


            if(Vector3.Distance(t.position, target.transform.position) < 2f)
            {
                horizontalInput = 1f;
                verticalInput = 1f;
            }

        }
        else
        {
            if (targetedWeapon)
            {
                Vector3 dir = (targetedWeapon.transform.position - t.position).normalized;

                horizontalInput = dir.x;
                verticalInput = dir.z;
            }
        }

        if (currentWeapon.GetType() == typeof(ContinuousWeapon))
        {
            //print(mouseDownInput);
            if (target)
            {
                mouseDownInput = true;
            }
            else
            {
                mouseDownInput = false;
            }
        }
        else
        {

            if (target)
            {
                mouseClickInput = 1;
            }
            else
            {
                mouseClickInput = -1;
            }
        }
    }



    protected override void Rotate()
    {

        if (target)
        {
            playerCam.transform.LookAt(target.transform);
        }
        else
        {
            playerCam.transform.localEulerAngles = t.localEulerAngles;
        }

        //base.Rotate();
    }


    protected override void Jump()
    {
        base.Jump();

        if (Physics.Raycast(t.position, t.forward, 1f, whatIsGround))
        {
            rb.AddForce(Vector3.up * jumpForce * jumpInput, ForceMode.Impulse);
        }

        if (Physics.Raycast(t.position, -t.forward, 1f, whatIsGround))
        {
            rb.AddForce(Vector3.up * jumpForce * jumpInput, ForceMode.Impulse);
        }
    }
}
