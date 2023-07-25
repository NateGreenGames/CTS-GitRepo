using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBoatHealthTest : MonoBehaviour
{
    public GameObject sailBoat;
    public int boatHealth;

    public Vector3 boatRespawnLocation;
    public Vector3 boatRespawnRotation;
    // Start is called before the first frame update
    void Start()
    {
        sailBoat = gameObject;
        boatHealth = 100;
        boatRespawnLocation = sailBoat.transform.position;
    }

    public void OnBoatDestroyed()
    {
        StartCoroutine(BoatDestroyCoroutine());
    }

    IEnumerator BoatDestroyCoroutine()
    {
        Debug.Log("initiating respawn...");
        yield return new WaitForSeconds(0f);
        gameObject.transform.position = boatRespawnLocation;
        gameObject.transform.rotation = Quaternion.Euler(boatRespawnRotation);
    }

}
