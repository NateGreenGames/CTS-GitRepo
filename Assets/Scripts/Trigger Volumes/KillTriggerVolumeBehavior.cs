using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTriggerVolumeBehavior : MonoBehaviour
{
    [SerializeField] private bool isHidden = true;

    // Start is called before the first frame update
    void Start()
    {
        if (isHidden)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().onDeath();
        }
    }
}