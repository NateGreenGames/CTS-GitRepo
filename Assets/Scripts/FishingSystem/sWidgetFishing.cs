using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sWidgetFishing : MonoBehaviour
{
    public GameObject fishWait;
    public GameObject fishOn;
    public GameObject reelIn;
    public GameObject fishFight;
    public GameObject lostFish;
    public GameObject gotFish;

    // Start is called before the first frame update
    void Start()
    {
        fishOn.SetActive(false);
        reelIn.SetActive(false);
        fishFight.SetActive(false);
        lostFish.SetActive(false);
        gotFish.SetActive(false);
        fishWait.SetActive(true);
        StartCoroutine(FishingTimer());

    }

    IEnumerator FishingTimer()
    {
        yield return new WaitForSeconds(30);
        fishOn.SetActive(false);
        reelIn.SetActive(false);
        fishFight.SetActive(false);
        gotFish.SetActive(false);
        fishWait.SetActive(false);
        lostFish.SetActive(true);
        yield return new WaitForSeconds(2);
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        Destroy(this.gameObject);

    }
    public void CompletedFish()
    {
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        Destroy(this.gameObject);
    }
}
