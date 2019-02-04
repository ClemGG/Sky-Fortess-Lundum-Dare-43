using UnityEngine;
using System.Collections;
using System;

public class RotatePlayerCamera : MonoBehaviour
{


    [Space]

    [SerializeField] float sensitivityX = 5; //reglage de la sensibilité de la souris en x
    [SerializeField] float sensitivityY = 5; //reglage de la sensibilité de la souris en y


    float minimumX = -360; //le min et max de rotation que l'on peux avoir sur x
    float maximumX = 360;
    [SerializeField] float minimumY = -60; //le min et le max de rotation que l'on peux avoir sur y
    [SerializeField] float maximumY = 60;
    
    float rotationX = 0; //la rotation de depart en x
    float rotationY = 0; //la rotation de depart en y
    Quaternion originalRotation; //Quaternion contenant la rotation de départ

    public MovementSystem player1;
    

    void Start()
    {

        originalRotation = transform.localRotation; //la rotation de depart est la rotation locale du transform
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(!PauseMenuButtons.instance.isGamePaused && player1.canMove)
            RotateCamera();
    }


    private void RotateCamera()
    {
        //on recupere les getaxis de la souris, on multiplie par la sensibilité et on calcule la rotation
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        //on prepare la rotation en x autour de l'axe y
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        //on prepare la rotation autour de l'axe x
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        //on effectue la rotation
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
