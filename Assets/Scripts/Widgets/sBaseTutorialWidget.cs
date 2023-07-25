using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sBaseTutorialWidget : MonoBehaviour
{
    public GameObject[] objective;
    public GameObject[] objectiveCheckbox;
    public GameObject[] objectiveText;
    public GameObject topText;
    public int numOfActiveObjs;
    public int numOfCompleteObjs;
    public bool tutorialComplete;


    public void SetTopText(string inputString)
    {
        topText.GetComponent<TextMeshProUGUI>().text = inputString;
    }

    public void SetObjAsActive(int obj) //Make sure that when activating objectives you use a for loop to activate them in order.
    {
        objective[obj].SetActive(true);
        numOfActiveObjs += 1;
    }

    public void SetObjectiveText(int obj,string inputString)
    {
        objectiveText[obj].GetComponent<TextMeshProUGUI>().text = inputString;
    }

    public void CompleteObjective(int obj)
    {
        objectiveCheckbox[obj].GetComponent<Toggle>().isOn = true;
        numOfCompleteObjs += 1;
        CompletionCheck();
    }

    public void CompletionCheck()
    {
        if (numOfCompleteObjs == numOfActiveObjs)
        {
            tutorialComplete = true;
            StartCoroutine(DestroyWidget());
        }
    }

    IEnumerator DestroyWidget()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
    
}
