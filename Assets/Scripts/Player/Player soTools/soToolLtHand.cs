using UnityEngine;

[CreateAssetMenu(fileName = "New ToolLtHand", menuName = "Create ToolLtHand")]
public class soToolLtHand : ScriptableObject
{
    public eToolLtHand toolLtHand;
    public bool isEquipped;
    public string sTool;
    public GameObject pTool;
}
