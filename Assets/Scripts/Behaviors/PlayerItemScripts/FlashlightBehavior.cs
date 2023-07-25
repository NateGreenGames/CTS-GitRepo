using UnityEngine;

public class FlashlightBehavior : MonoBehaviour
{
    [SerializeField] private bool isOn;
    public GameObject lightSource;

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        lightSource.SetActive(isOn);
    }
}
