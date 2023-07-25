using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sLantern : MonoBehaviour, IInteractable
{
    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }
    public Light lanternLight;
    public sLanternPuzzleManager lanternManager;
    public bool lanternActive;


    public void OnInteract()
    {
        StartCoroutine(ToggleLampRoutine());
    }

    public void OnLookingAt()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        isInteractable = true;
        lanternLight.enabled = false;
        lanternActive = false;
        HudMessage = "Interact to Light Lantern";
    }

    IEnumerator ToggleLampRoutine()
    {
        lanternLight.enabled = !lanternLight.enabled;
        lanternActive = !lanternActive;
        isInteractable = false;
        yield return new WaitForSeconds(1);
        lanternManager.LanternCheck();
    }
}
