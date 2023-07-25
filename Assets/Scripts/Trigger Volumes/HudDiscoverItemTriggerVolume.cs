using UnityEngine;

public class HudDiscoverItemTriggerVolume : MonoBehaviour
{
    CanvasHUD canvasHUD;

    [SerializeField] int widgetToSpawn;

    void Start()
    {
        canvasHUD = GameManager.gm.canvasManager.canvasHUD.GetComponent<CanvasHUD>();
    }

    private void OnTriggerEnter(Collider other)
    {
        canvasHUD.tutorialHUD.DiscoverItemUI(widgetToSpawn);
    }

    private void OnTriggerExit(Collider other)
    {
        //canvasHUD.tutorialHUD.ResetTutorials();
    }
}
