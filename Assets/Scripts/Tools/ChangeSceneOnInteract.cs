using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnInteract : MonoBehaviour, IInteractable
{
    public string sceneToChangeTo = "CTS_WorldScene";
    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }
    private GameManager gm;
    private void Start()
    {
        gm = GameManager.gm;
        HudMessage = "Interact with Boat";
        isInteractable = true;
    }

    public void OnLookingAt()
    {

    }
    public void OnInteract()
    {
        StartCoroutine(gm.fadeManager.ChangeScenes("CTS_WorldScene", 1.5f, 1.5f));
    }
}
