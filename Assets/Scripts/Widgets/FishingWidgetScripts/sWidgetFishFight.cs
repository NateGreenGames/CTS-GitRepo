using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sWidgetFishFight : MonoBehaviour
{
    sWidgetFishing widgetFishing;
    public GameObject leftTriggerSprite;
    public GameObject rightTriggerSprite;
    
    private int stepCounter;
    // Start is called before the first frame update
    void Start()
    {
        leftTriggerSprite.SetActive(false);
        rightTriggerSprite.SetActive(false);
        stepCounter = 0;
        widgetFishing = GetComponentInParent<sWidgetFishing>();
        StartCoroutine(FishSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if (stepCounter == 0 && Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            stepCounter = 1;
        }
        if (stepCounter == 1 && Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            stepCounter = 2;
        }
        if (stepCounter == 2 && Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            stepCounter = 3;
        }
        if (stepCounter == 3 && Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            widgetFishing.fishFight.SetActive(false);
            widgetFishing.gotFish.SetActive(true);
        }
    }

    IEnumerator RightPulse()
    {
        Gamepad.current.SetMotorSpeeds(0f, 0.75f);
        rightTriggerSprite.SetActive(true);
        yield return new WaitForSeconds(1);
        rightTriggerSprite.SetActive(false);
        Gamepad.current.ResetHaptics();
    }

    IEnumerator LeftPulse()
    {
        Gamepad.current.SetMotorSpeeds(0.75f, 0f);
        leftTriggerSprite.SetActive(true);
        yield return new WaitForSeconds(1);
        leftTriggerSprite.SetActive(false);
        Gamepad.current.ResetHaptics();
    }
    IEnumerator FishSequence()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(LeftPulse());
        yield return new WaitForSeconds(1);
        StartCoroutine(RightPulse());
        yield return new WaitForSeconds(2);
        StartCoroutine(RightPulse());
        yield return new WaitForSeconds(1);
        StartCoroutine(LeftPulse());
        yield return new WaitForSeconds(1);



    }
}
