using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sCabinTutorialManager : MonoBehaviour
{
    public GameObject getToCabinTrigger;
    public GameObject getToBoatTrigger;
   
    private GameObject tutorialWidget;
    private sBaseTutorialWidget widgetScript;

    // Start is called before the first frame update
    void Start()
    {
        tutorialWidget = Instantiate(Resources.Load("Widgets/" + "Widget_Tutorial") as GameObject);
        tutorialWidget.transform.SetParent(GameObject.FindGameObjectWithTag("TestCanvas").transform, false);
        widgetScript = tutorialWidget.GetComponent<sBaseTutorialWidget>();
        widgetScript.SetTopText("Objectives");
        for (int i = 0; i < 3; i++)
        {
            widgetScript.SetObjAsActive(i);
        }
        widgetScript.SetObjectiveText(0, "Get to the Cabin");
        widgetScript.SetObjectiveText(1, "Explore the Cabin");
        widgetScript.SetObjectiveText(2, "Get on the Boat");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.isToolUnlocked[1] == true && GameManager.gm.isToolUnlocked[2] == true && GameManager.gm.isToolUnlocked[3] == true && widgetScript.objectiveCheckbox[1].GetComponent<Toggle>().isOn == false)
        {
            ExploreCabinComplete();
        }
    }

    public void GetToCabinComplete()
    {
        widgetScript.CompleteObjective(0);
    }
    public void ExploreCabinComplete()
    {
        widgetScript.CompleteObjective(1);
    }
    public void GetToBoatComplete()
    {
        widgetScript.CompleteObjective(2);
        StartCoroutine(DestroyManager());
    }
    IEnumerator DestroyManager()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
