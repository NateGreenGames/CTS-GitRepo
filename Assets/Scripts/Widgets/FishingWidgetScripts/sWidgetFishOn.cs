using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sWidgetFishOn : MonoBehaviour
{
    sWidgetFishing widgetFishing;

    // Start is called before the first frame update
    void Start()
    {
        widgetFishing = GetComponentInParent<sWidgetFishing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.aButton.wasPressedThisFrame)
        {
            widgetFishing.reelIn.SetActive(true);
            widgetFishing.fishOn.SetActive(false);
        }
    }
}
