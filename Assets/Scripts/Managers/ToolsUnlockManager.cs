using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsUnlockManager : MonoBehaviour
{
    public PlayerProgressionUnlock playerToolUnlock;

    // Update is called once per frame

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Delete(playerToolUnlock);
            Debug.Log("Json File Deleted!");
        }
    }

    void Delete(PlayerProgressionUnlock playerToolUnlock)
    {
        File.Delete(Application.persistentDataPath + "/pToolUnlock.json");
    }


}
