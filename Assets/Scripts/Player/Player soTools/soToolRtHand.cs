using UnityEngine;

[CreateAssetMenu(fileName = "New ToolRtHand", menuName = "Create ToolRtHand")]
public class soToolRtHand : ScriptableObject
{
    public eToolRtHand toolRtHand;
    public bool isEquipped;
    public bool isUnlocked;
    public string sTool;
    public GameObject pTool;

    // public protected int toolID;
}
