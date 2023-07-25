using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillCollider : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public sBoatHealthTest boatHealthTest;

    private void Start()
    {
        boatHealthTest = GameObject.FindGameObjectWithTag("Ship").GetComponent<sBoatHealthTest>();
        sphereCollider = this.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the ship enters the second collider located at the center of the whirlpool,
        // the boat is destroyed and then boat/player respawns
        if (other.gameObject.tag == "Ship")
        {
            Debug.Log("Instantkill field entered");
            boatHealthTest.boatHealth -= 100;
            Debug.Log($"Ship Taking damage. Current health is at {boatHealthTest.boatHealth}");
            if (boatHealthTest.boatHealth <= 0)
            {
                Debug.Log("Boat Destroyed");
                // If there will be a respawn screen, this can be altered to be called after a time or when a button is clicked
                boatHealthTest.OnBoatDestroyed();
                boatHealthTest.boatHealth = 100;
            }
        }

    }
    public void SetColliderProperties(float radius, float centerOffset)
    {
        sphereCollider.radius = radius;
        sphereCollider.center = new Vector3(0f, -centerOffset, 0f);
    }
}
