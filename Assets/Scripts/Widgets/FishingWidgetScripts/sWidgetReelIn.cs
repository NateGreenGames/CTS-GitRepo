using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sWidgetReelIn : MonoBehaviour
{
    sWidgetFishing widgetFishing;
    private int reelCounter;
    public int reelMax = 50;
    // Start is called before the first frame update
    void Start()
    {
        reelCounter = 0;
        widgetFishing = GetComponentInParent<sWidgetFishing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftStick.left.wasPressedThisFrame|| Gamepad.current.leftStick.right.wasPressedThisFrame)
        {
            reelCounter += 1;
            Debug.Log(reelCounter);
        }
        if (reelCounter > reelMax)
        {
            widgetFishing.reelIn.SetActive(false);
            widgetFishing.fishFight.SetActive(true);
        }
    }
}
