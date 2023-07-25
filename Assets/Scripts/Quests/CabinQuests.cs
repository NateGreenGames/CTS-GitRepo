using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinQuests : MonoBehaviour
{

    [SerializeField] private bool isHidden = true;
    void Start()
    { 
        if (isHidden)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager.gm.questManager.GetQuest(13);
    }
}
