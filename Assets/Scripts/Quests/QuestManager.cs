using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public QuestSO[] questArray;
    public static UnityAction<string> functionMessageToSend;


    #region Quest Variables for current quest
    public int localQuestId;
    public string localQuestTitle;
    public string localQuestDesc;
    public Vector3 localWaypointLocation;
    public int localCurrentProg;
    public int localMaxProg;
    public string localMessage;
    #endregion

    #region Values for the lerp function
    private float timeElapsed = 0;
    private float lerpDuration = 120f;
    private float startValue = 12.5f;
    private float endValue = 23.5f;
    private float valueToLerp = 0;
    #endregion

    

    // Start is called before the first frame update
    void Start()
    {
        //TODO:Eventually tie this into the save system to grab the quest progress from a save.
    }


    //GetQuest will grab the information from the quest specified by the quest ID and hold that information locally for the game to modify as needed.
    public void GetQuest(int _questId)
    {
        if((int)GameManager.gm.curScene >= 2)
        {
            localQuestId = questArray[_questId].questId;
            localQuestTitle = questArray[_questId].questTitle;
            localQuestDesc = questArray[_questId].questDescription;
            localWaypointLocation = questArray[_questId].waypointLocation;
            localCurrentProg = questArray[_questId].currentProg;
            localMaxProg = questArray[_questId].maxProg;
            localMessage = questArray[_questId].functionMessage;
        }
        if (localMessage != "none")
        {
            functionMessageToSend.Invoke(localMessage);
        }
        UpdateQuestUI();
        UpdateCoords();
    }

    public void UpdateQuestUI()
    {
        //TODO: Wire this into a quest HUD
        GameManager.gm.canvasManager.canvasHUD.questHUD.SetActive(true);
        GameManager.gm.canvasManager.canvasHUD.GetComponent<CanvasHUD>().UpdateQuestHUD(localQuestTitle, localQuestDesc, localCurrentProg, localMaxProg);
        Debug.Log(localQuestTitle);
        Debug.Log("You are on step " + localCurrentProg + " of " + localMaxProg);

    }

    [ContextMenu("AdvanceStep")]
    public void AdvanceStep()
    {
        localCurrentProg += 1;
        if (localCurrentProg == localMaxProg)
        {
            GetQuest(localQuestId + 1);
            UpdateQuestUI();
        }
        else if (localCurrentProg < localMaxProg)
        {
            UpdateQuestUI();
        }

    }

    public void UpdateCoords()
    {
        AstrolabeBehavior astrolabeRef = GameManager.gm.playerReference.GetComponentInChildren<AstrolabeBehavior>();
          if (astrolabeRef != null)  astrolabeRef.ChangeHeading(localWaypointLocation);
    }

    private IEnumerator TimeLerp()
    {
        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            Enviro.EnviroManager.instance.Time.SetTimeOfDay(valueToLerp);
            yield return null;
        }
    }


}
