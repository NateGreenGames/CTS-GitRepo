using UnityEngine;
[System.Serializable]
// TODO: Consolidate the bools into an array
public class PlayerProgressionUnlock
{
    public static PlayerProgressionUnlock playerProgressionUnlock;
    // This scripts keeps track of what has been unlocked throught the game
    public bool haveSavedFile;
    public bool newGame;

    // Player Information
    public int curPlayerHealth;
    public int curBoatHealth;

    // Tools
    public int curFlares;
    public bool flareGunLoaded;

    public bool isFlareGunUnlocked;
    public bool isFlashlightUnlocked;
    public bool isAstrolabeUnlocked;
    public bool isLightningBottleUnlocked;
    public bool isLightningBottleCharged;
    // Reserved for the rest of the tools

    // Current Checkpoints
    public Vector3 respawnLocation;
    public Quaternion respawnRotation;

    // Change this to be an enum in the future/array
    public bool checkpoint_Letter;
    public bool checkPoint_Boat;
    public bool checkPoint_MainCabin;
    public bool checkPoint_Sailing;
    public bool checkPoint_Island;

    // Dad Notes

    //NPC Status
    public bool gryffnFirstVisit;
    public bool calliopeFirstVisit;
}

