using UnityEngine;

public class BoatTutorialManager : MonoBehaviour
{
    public bool firstTimeSceneLoad = true;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = false;
        Instantiate(Resources.Load("Widgets/Widget_BoatTutorial"));
        GameManager.gm.questManager.GetComponent<QuestManager>().GetQuest(0);
        GameManager.gm.questManager.GetComponent<QuestManager>().UpdateCoords();
        firstTimeSceneLoad = false;
    }
}
