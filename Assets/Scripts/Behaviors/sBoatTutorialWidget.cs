using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sBoatTutorialWidget : MonoBehaviour
{
    void Update()
    {
       if (GameManager.gm.gameObject.GetComponent<InputManager>().onFoot.SouthButton.triggered)
        {
            GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
            GameManager.gm.boatReference.GetComponent<ControllerManager>().ToggleControlsBetweenBoatAndPlayer();
            GameManager.gm.inputManager.SailingActionsToggle();
            Destroy(this.gameObject);
        } 
    }
}
