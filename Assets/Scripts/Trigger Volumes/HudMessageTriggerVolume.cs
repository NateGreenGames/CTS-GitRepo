using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudMessageTriggerVolume : MonoBehaviour
{
    [SerializeField] private bool isHidden = true;
    [SerializeField] private string messageToDisplay;
    private CanvasHUD canvasHUD;

    // Start is called before the first frame update
    void Start()
    {
        canvasHUD = GameManager.gm.canvasManager.canvasHUD;
        if (isHidden)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        canvasHUD.hudFunctions.SetMissionText("", messageToDisplay, false);
    }

    private void OnTriggerExit(Collider other)
    {
        canvasHUD.hudFunctions.SetMissionText("", "", false);
        this.gameObject.SetActive(false);
    }
}
