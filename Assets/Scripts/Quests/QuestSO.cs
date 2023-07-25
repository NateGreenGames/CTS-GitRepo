using UnityEngine;

[CreateAssetMenu(fileName = "Quests", menuName = "Create Quest")]


public class QuestSO : ScriptableObject
{
    [Header("General Info")]
    public int questId;
    public string questTitle;
    public string questDescription;
    public Vector3 waypointLocation;
    [Header("Progress")]
    public int currentProg;
    public int maxProg;
    [Header("Functions")]
    public string functionMessage;

}
