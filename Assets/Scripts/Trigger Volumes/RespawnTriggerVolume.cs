using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTriggerVolume : MonoBehaviour
{
    [SerializeField] private bool isHidden = true;
    private GameObject spawnAnchor;
    private SaveSettings saveSettings;

    // Start is called before the first frame update
    void Start()
    {
        saveSettings = GameObject.Find("pGameManager").GetComponent<SaveSettings>();
        foreach (Transform obj in gameObject.transform)
        {
            spawnAnchor = obj.gameObject;
            if (spawnAnchor.name != "Respawn Position") Debug.Log($"{gameObject.name} contains a respawn trigger volume script but has more than one child.");
        }
        if (isHidden)
        {
            //Turns off the renderer on this object, then the renderer of all of the children of this object.
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            spawnAnchor.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerMovement playerRef = other.gameObject.GetComponent<PlayerMovement>();
            if(playerRef.respawnLocation != spawnAnchor.transform.position)
            {
                playerRef.respawnLocation = spawnAnchor.transform.position;
                playerRef.respawnRotation = spawnAnchor.transform.rotation;
                Debug.Log("Player spawn point updated.");

                saveSettings.Saving();
            }
        }
    }

    private void CheckScene()
    {

    }
}
