using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sGetToBoatTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<sCabinTutorialManager>().GetToBoatComplete();
            Destroy(this.gameObject);
        }
    }
}
