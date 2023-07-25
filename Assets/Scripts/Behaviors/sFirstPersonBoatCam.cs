using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sFirstPersonBoatCam : MonoBehaviour
{
    [SerializeField] private float fpsSensitivity;

    Vector3 angles;
    private SailboatBehavior boatRef;
    void Start()
    {
        boatRef = GameManager.gm.boatReference.GetComponent<SailboatBehavior>();
    }
    void Update()
    {
        if (boatRef.fpsCamOn == true)
        {
            float rotationY = Gamepad.current.rightStick.ReadValue().y * fpsSensitivity;
            float rotationX = Gamepad.current.rightStick.ReadValue().x * fpsSensitivity;

            if (rotationY > 0)
            {
                angles = new Vector3(Mathf.MoveTowards(angles.x, -80, rotationY), angles.y + rotationX, 0);
            }
            else
            {
                angles = new Vector3(Mathf.MoveTowards(angles.x, 80, -rotationY), angles.y + rotationX, 0);
                transform.localEulerAngles = angles;
            }
        }
    }
}
