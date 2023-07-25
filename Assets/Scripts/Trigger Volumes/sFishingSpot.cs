using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sFishingSpot : MonoBehaviour,IInteractable
{
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }
    public int spotRefreshTime; //Variable can be changed to determine how long it takes for spot to be interactible again.

    public void OnInteract()
    {
        isInteractable = false;
        GameObject widget = Instantiate(Resources.Load("Widgets/" + "Widget_Fishing") as GameObject);
        widget.transform.SetParent(GameObject.FindGameObjectWithTag("CanvasHUD").transform, false); 
        StartCoroutine(SpotRefresh());
    }

    public void OnLookingAt()
    {
        
    }

    IEnumerator SpotRefresh()
    {
        yield return new WaitForSeconds(spotRefreshTime);
        isInteractable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        isInteractable = true;
        HudMessage = "Press E to Fish";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
