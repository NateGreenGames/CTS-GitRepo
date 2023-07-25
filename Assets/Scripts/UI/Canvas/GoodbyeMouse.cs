using UnityEngine;
using UnityEngine.EventSystems;

public class GoodbyeMouse : MonoBehaviour
{
    GameObject lastSelect;

    private void Start()
    {
        lastSelect = new GameObject();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelect);
        }
        else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }
}
