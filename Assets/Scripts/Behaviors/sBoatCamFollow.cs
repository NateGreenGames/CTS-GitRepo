using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sBoatCamFollow : MonoBehaviour
{

    [SerializeField] private Vector3 offsetFromBoat;
    [SerializeField] private float cameraPanSpeed = 3.0f;//a speed modifier


    private SailboatBehavior boatRef;
    void Start()
    {
        boatRef = GameManager.gm.boatReference.GetComponent<SailboatBehavior>();
    }

    void Update()
    {
        if (boatRef.tpsCamOn == true)
        {
            Vector3 boatLocationPlusOffset = GameManager.gm.boatReference.transform.position + offsetFromBoat;
            transform.LookAt(boatLocationPlusOffset);//makes the camera look to it
                                                     //makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
            if (Gamepad.all.Count == 0)
            {
                Debug.Log("Please plug in controller!");
            }
            else
            {
                transform.RotateAround(boatLocationPlusOffset, new Vector3(-Gamepad.current.rightStick.ReadValue().y, Gamepad.current.rightStick.ReadValue().x, 0.0f), 20 * Time.deltaTime * cameraPanSpeed);
            }
        }
    }

}

