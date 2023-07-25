using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Repository", menuName = "Create Repository")]
public class soRep : ScriptableObject
{
    [NamedArray(typeof(eToolRtHand))] public soToolRtHand[] toolRtHand;
    [NamedArray(typeof(eToolLtHand))] public soToolLtHand[] toolLtHand;
}
