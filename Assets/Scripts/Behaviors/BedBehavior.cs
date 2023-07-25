using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBehavior : MonoBehaviour, IInteractable
{
    public string HudMessage {get; set;}
    public bool isInteractable { get; set; }

    private void Start()
    {
        HudMessage = "Press E to sleep.";
        isInteractable = true;
    }

    public void OnLookingAt()
    {

    }
    public void OnInteract()
    {
        GameManager gm = GameManager.gm;
        gm.enviroSystem.Time.SetTimeOfDay(gm.enviroSystem.Time.hours + 8);
        /*if(GameManager.gm.enviroSystem.Time.hours < 6 || GameManager.gm.enviroSystem.Time.hours > 18)
        {
            GameManager.gm.enviroSystem.Time.SetTimeOfDay(6);
        }else if(GameManager.gm.enviroSystem.Time.hours > 6 && GameManager.gm.enviroSystem.Time.hours < 18)
        {
            GameManager.gm.enviroSystem.Time.SetTimeOfDay(18);
        }*/
    }
}
