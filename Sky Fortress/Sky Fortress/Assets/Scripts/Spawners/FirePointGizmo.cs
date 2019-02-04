using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointGizmo : MonoBehaviour {


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.DrawSphere(transform.position, .1f);
    }
}
