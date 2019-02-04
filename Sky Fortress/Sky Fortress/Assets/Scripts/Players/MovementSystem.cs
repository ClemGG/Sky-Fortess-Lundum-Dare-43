using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Clement.Utilities.Conversions;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StatsSystem))]
public class MovementSystem : MonoBehaviour {

    [Header("Scripts & Components : ")]
    [Space(10)]

    protected Transform t;
    protected Rigidbody rb;
    [HideInInspector] public StatsSystem stats;

    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [SerializeField] protected Camera playerCam;
    protected float horizontalInput;
    protected float verticalInput;
    protected float jumpInput;
    public int mouseClickInput = 0;
    public bool mouseDownInput = false;




    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpForce;

    [SerializeField] protected LayerMask whatIsGround;


    [Space(10)]
    [Header("Weapons : ")]
    [Space(10)]

    public int joueurID;
    public bool canMove = true;

    public Weapon currentWeapon;
    public Weapon lastWeapon;
    public Transform weaponPos;




    protected virtual void Start () {
        
        t = transform;

        if(!playerCam)
            playerCam = t.GetChild(0).GetComponent<Camera>();

        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsSystem>();

        if (currentWeapon)
        {
            currentWeapon.weaponID = joueurID;

            if(joueurID == 1)
                ScoreManager.instance.UpdateWeaponUI();
        }
        

    }

    protected void Update () {

        if (!GetComponent<StatsSystem>().isDead && canMove)
        {
            GetInput();
        }
	}

    protected virtual void FixedUpdate () {

        if (!GetComponent<StatsSystem>().isDead && canMove)
        {
            Move();
            Jump();
            Rotate();
            Shoot();
        }
	}













    protected virtual void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetButtonDown("Jump").ToFloat(false);


        if (Input.GetMouseButton(0))
        {
            mouseClickInput = 1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownInput = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDownInput = false;
            mouseClickInput = -1;
        }
        //else
        //{
        //    mouseClickInput = -2;
        //}
    }










    protected void Move()
    {

        t.Translate(t.forward * verticalInput * moveSpeed * Time.deltaTime);
        t.Translate(t.right * horizontalInput * moveSpeed * Time.deltaTime);
    }










    protected virtual void Jump()
    {
        if(Physics.Raycast( t.position, -t.up, 1f, whatIsGround))
        {
            rb.AddForce(Vector3.up * jumpForce * jumpInput, ForceMode.Impulse);
        }
    }










    protected virtual void Rotate()
    {
        if(playerCam)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, playerCam.transform.localEulerAngles.y, transform.localEulerAngles.z);

    }











    protected void Shoot()
    {
        if (!currentWeapon)
            return;


        if (currentWeapon.timer < currentWeapon.fireRate)
        {
            currentWeapon.timer += Time.deltaTime;
        }
        else
        {

            if (currentWeapon.GetType() == typeof(ContinuousWeapon))
            {
                //print(mouseDownInput);
                if (mouseDownInput)
                {
                    currentWeapon.Shoot();
                }
                else
                {
                    currentWeapon.ReleaseBullet();
                }
            }
            else
            {

                if (mouseClickInput == 1)
                {
                    currentWeapon.Shoot();
                }
            }
        }
    }







    protected void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "WeaponObject")
        {
            
            if (!c.gameObject.GetComponent<WeaponObject>().canBePickedUp || c.gameObject.GetComponent<Weapon>().weaponType == currentWeapon.weaponType)
                return;

            if (c.transform.childCount > 1)
            {
                if (c.transform.GetChild(1).GetComponent<ParticleSystem>())
                {
                    Destroy(c.transform.GetChild(1).gameObject);
                }
            }

            PickUpNewWeapon(c.transform);
            DropLastWeapon();

        }
    }




    protected void PickUpNewWeapon(Transform c)
    {

        Destroy(c.GetComponent<WeaponObject>());
        Destroy(c.GetComponent<Rigidbody>());
        Destroy(c.GetComponent<BoxCollider>());

        lastWeapon = currentWeapon;
        c.position = weaponPos.position;
        c.rotation = weaponPos.rotation;
        c.parent = weaponPos;

        if (currentWeapon.GetType() == typeof(ContinuousWeapon))
        {
            currentWeapon.ReleaseBullet();
        }


        currentWeapon = c.GetComponent<Weapon>();
        currentWeapon.weaponID = joueurID;

        if(joueurID == 1)
        {
            ScoreManager.instance.UpdateWeaponUI();
        }

    }

    public void DropLastWeapon()
    {

        if (lastWeapon.GetType() == typeof(ContinuousWeapon))
        {
            lastWeapon.ReleaseBullet();
        }

        if (!lastWeapon.name.Contains("(Clone)"))
            lastWeapon.gameObject.SetActive(false);
        else
        {
            lastWeapon.transform.parent = null;
            lastWeapon.gameObject.AddComponent<WeaponObject>();
            lastWeapon.gameObject.GetComponent<WeaponObject>().OnObjectSpawn(lastWeapon);
        }
        
    }



    protected void OnDrawGizmos()
    {
        if (playerCam == null)
            return;

        Vector3 camDir = new Vector3(0f, 0f, playerCam.transform.forward.z).normalized;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCam.transform.position, playerCam.transform.forward);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(playerCam.transform.position, camDir);
    }
}
