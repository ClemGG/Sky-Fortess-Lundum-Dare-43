using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BlueLaser : MonoBehaviour {

    [Header("Scripts & Components : ")]
    [Space(10)]

    protected Transform t;



    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [HideInInspector] public int bulletID;
    public int dmgToDeal;
    public float laserDelay = .3f;


    [SerializeField] private GameObject[] objectsAtTheEndOfTheLaser;
    private LineRenderer line;
    private RaycastHit hit;
    [SerializeField] private LayerMask whatIsDetectable;


    // Use this for initialization
    void Start () {
        t = transform;
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Physics.Raycast(t.parent.position, t.forward, out hit, 300f, whatIsDetectable, QueryTriggerInteraction.Ignore))
        {
            line.SetPosition(0, t.parent.position);
            line.SetPosition(1, hit.point);

            for (int i = 0; i < objectsAtTheEndOfTheLaser.Length; i++)
            {
                objectsAtTheEndOfTheLaser[i].transform.position = line.GetPosition(1);

            }
        }
        else
        {
            line.SetPosition(0, t.parent.position);
            line.SetPosition(1, transform.forward * 300f);
        }
	}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.forward *10f);
        Gizmos.DrawSphere(hit.point, .2f);
    }
}
