
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class WeaponObject : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 OnSpawnForce;
    private Transform t;

    public bool canBePickedUp = true;

    private void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    void FixedUpdate () {
        t.RotateAround(t.position, t.up, rotationSpeed * Time.deltaTime);
	}

    public void OnObjectSpawn(Weapon lastWeapon)
    {
        gameObject.tag = "WeaponObject";

        Rigidbody rb = GetComponent<Rigidbody>();

        if(lastWeapon.GetType() == typeof(ContinuousWeapon))
        {
            GetComponent<BoxCollider>().size = new Vector3(1.5f, 1.5f, 1.5f);

        }
        else
        {
            GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);

        }

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.mass = 100f;

        transform.rotation = Quaternion.identity;

        //if (lastWeapon.GetType() == typeof(ContinuousWeapon))
        //{
        //    GetComponent<ContinuousWeapon>().NewWeapon(lastWeapon);
        //}
        //else if (lastWeapon.GetType() == typeof(ProjectileWeapon))
        //{
        //    GetComponent<ProjectileWeapon>().NewWeapon(lastWeapon);
        //}



        canBePickedUp = false;
        OnSpawnForce = new Vector3(Random.Range(0f, 3f), Random.Range(0f, 3f), Random.Range(0f, 3f)) * rb.mass;
        rb.AddRelativeForce(OnSpawnForce, ForceMode.Impulse);
        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1f);
        canBePickedUp = true;

    }
}
