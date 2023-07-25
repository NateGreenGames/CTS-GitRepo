using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sLanternPuzzleManager : MonoBehaviour
{
    public GameObject[] lanterns;
    public int numLanterns;
    public bool puzzleSolved;
    public GameObject effectObject;

    private int lanternCounter;

    void Start()
    {
        lanternCounter = 0;
        numLanterns = lanterns.Length;

    }

    public void LanternCheck()
    {
        if (lanterns[lanternCounter].GetComponent<sLantern>().lanternActive == true)
        {
            lanternCounter += 1;
        }
        else
        {
            foreach (var lantern in lanterns)
            {
                sLantern lanternRef = lantern.GetComponent<sLantern>();
                IInteractable interactableRef = lantern.GetComponent<IInteractable>();
                lanternRef.lanternLight.enabled = false;
                lanternRef.lanternActive = false;
                interactableRef.isInteractable = true;
                
            }
            lanternCounter = 0;
        }
        PuzzleSolveCheck();

    }
    private void PuzzleSolveCheck()
    {
        if (lanterns[numLanterns - 1].GetComponent<sLantern>().lanternActive == true)
        {
            puzzleSolved = true;
            //TODO Make this a generic thing that can apply to any object
            effectObject.GetComponent<sChest>().OpenChest();
        }
    }

}
