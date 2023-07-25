using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sDamageRocks : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // TODO: Actually make this relevant to the boat health in terms of the boat being destroyed.
        if (other.tag == "Ship")
        {
            other.GetComponent<sBoatHealthTest>().boatHealth -= 20;
            if (other.GetComponent<sBoatHealthTest>().boatHealth <= 0)
            {
                Debug.Log("The boat is deadge");
            }
        }
        
    }
    
}
