using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sWidgetFishWait : MonoBehaviour
{
    sWidgetFishing widgetFishing;
    // Start is called before the first frame update
    void Start()
    {
        widgetFishing = GetComponentInParent<sWidgetFishing>();
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        int randomWait = Random.Range(1, 3);
        StartCoroutine(RandomFishWait(randomWait));

    }

    IEnumerator RandomFishWait(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Gamepad.current.SetMotorSpeeds(0.75f, 0.75f);
        yield return new WaitForSeconds(1);
        Gamepad.current.ResetHaptics();
        widgetFishing.fishWait.SetActive(false);
        widgetFishing.fishOn.SetActive(true);
    }
}
