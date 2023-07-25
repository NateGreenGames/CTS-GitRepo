using UnityEngine;
using System.IO;


public class LeftItemSwapping : MonoBehaviour
{
    public PlayerProgressionUnlock pToolUnlocked;

    public Transform leftToolContainer;
    public int leftSelectedTool;
    public int previousSelectedTool;

    public soRep sRep;

    GameManager gm;
    string pToolUnlockPath;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        LeftItemSwap(sRep);
        LeftToolSpawn(sRep);
    }

    // Update is called once per frame
    void Update()
    {
        // Saves the tool unlock states to an external .json file that persists through plays
        // Press F12 to delete the .json file to set all unlock states for tools back to false
        // F12 input located in the ToolsUnlockManager Script
        pToolUnlockPath = $"{Application.persistentDataPath}/pToolUnlock.json";
        if (File.Exists(pToolUnlockPath))
        {
            string json = File.ReadAllText(pToolUnlockPath);
            pToolUnlocked = JsonUtility.FromJson<PlayerProgressionUnlock>(json);
        }
        LeftSwitchingTools();
    }

    public void LeftToolSpawn(soRep _soRep)
    {
        // Spawns in all tools in the toolRtHand array
        for (int i = 0; i < _soRep.toolLtHand.Length; i++)
        {
            int x = 0, y = 0, z = 0;
            GameObject obj = Instantiate(_soRep.toolLtHand[i].pTool, leftToolContainer.transform.position, transform.rotation * Quaternion.Euler(x, y, z));
            obj.transform.parent = leftToolContainer;
        }
        LeftItemSwap(sRep);
    }
    public void LeftItemSwap(soRep _soRep)
    {
        int a = 0;
        // Cycles through the tools in the left hand
        foreach (Transform tool in transform)
        {
            
            // Checks the GameManager to see if tool is unlocked, if not increments to the next unlocked tool
            if (a == leftSelectedTool)
            {
                tool.gameObject.SetActive(true);
                _soRep.toolLtHand[a].isEquipped = true;
            }
            else
            {
                tool.gameObject.SetActive(false);
                _soRep.toolLtHand[a].isEquipped = false;
            }
            a++;
        }
    }

    public void LeftSwitchingTools()
    {
        int leftPreviousSelectedTool = leftSelectedTool;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LeftNextItemSwap();
        }
        if (leftPreviousSelectedTool != leftSelectedTool)
        {
            LeftItemSwap(sRep);
        }
    }

    public void LeftNextItemSwap()
    {
        if (leftSelectedTool >= transform.childCount - 1) leftSelectedTool = 0;
        else leftSelectedTool++;
    }

}
