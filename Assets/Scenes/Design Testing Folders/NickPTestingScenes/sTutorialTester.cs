using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sTutorialTester : MonoBehaviour
{
    private GameObject tutorialWidget;
    private sBaseTutorialWidget widgetScript;
    // Start is called before the first frame update
    void Start()
    {
        tutorialWidget = Instantiate(Resources.Load("Widgets/" + "Widget_Tutorial") as GameObject);
        tutorialWidget.transform.SetParent(GameObject.FindGameObjectWithTag("TestCanvas").transform, false);
        widgetScript = tutorialWidget.GetComponent<sBaseTutorialWidget>();
        widgetScript.SetTopText("Hi Caleb");
        for (int i = 0; i < 2; i++)
        {
            widgetScript.SetObjAsActive(i);
        }
        widgetScript.SetObjectiveText(0, "Hi");
        widgetScript.SetObjectiveText(1, "Caleb");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            widgetScript.CompleteObjective(0);
        }
        if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            widgetScript.CompleteObjective(1);
        }

    }
}
