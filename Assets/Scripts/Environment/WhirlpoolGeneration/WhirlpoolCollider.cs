using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WhirlpoolCollider : MonoBehaviour
{
    public sBoatHealthTest boatHealthTest;
    public WhirlpoolGeneration whirlpoolGeneration;
    public SphereCollider sphereCollider;

    private int damage = 10; // The amount of damage to deal.
    private float damageInterval = 10.0f; // The time interval between damage ticks.
    private float nextDamageTime = 0.0f; // The time when the next damage tick can occur.

    // Start is called before the first frame update
    void Start()
    {
        boatHealthTest = GameObject.FindGameObjectWithTag("Ship").GetComponent<sBoatHealthTest>();
        whirlpoolGeneration = GameObject.FindGameObjectWithTag("WhirlpoolGenerator").GetComponent<WhirlpoolGeneration>();
        sphereCollider = this.GetComponent<SphereCollider>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ship")
        {
            // Check if enough time has passed since the last damage tick.
            if (Time.time >= nextDamageTime)
            {
                // Deal damage to the player.
                boatHealthTest.boatHealth -= damage;
                Debug.Log($"Ship Taking damage. Current health is at {boatHealthTest.boatHealth}");

                // Needs code to respawn boat with player

                // Update the time when the next damage tick can occur.
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

   public void SetDamageColliderProperties(float radius)
    {
        sphereCollider.radius = radius;
    }
}
