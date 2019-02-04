using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

    [SerializeField] private float rotationSpeed;
    Transform t;

    private void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    void Update () {
        t.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
