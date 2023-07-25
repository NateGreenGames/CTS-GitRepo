using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBoatFlare : MonoBehaviour
{
    public GameObject flarePrefab;
    public Transform flareSpawnPos;
    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootBoatFlare()
    {
        GameObject flare = Instantiate(flarePrefab, flareSpawnPos.position, flareSpawnPos.rotation);
        flare.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, projectileSpeed, 0));

    }
}
