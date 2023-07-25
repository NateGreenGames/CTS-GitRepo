using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TabButtons : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    //This code can be found here: https://www.youtube.com/watch?v=211t6r12XPQ

    public TabArea tabArea;
    public Image background;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    public void OnPointerClick(PointerEventData eventData)
    {
        tabArea.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabArea.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabArea.OnTabExit(this);
    }

    private void Start()
    {
        background = GetComponent<Image>();
        tabArea.Subscribe(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

    public void ColorButtonYellow()
    {
        background.color = Color.yellow;
    }

    public void ColorButtonWhite()
    {
        background.color = Color.white;
    }

    public void StretchTabScale()
    {
        background.transform.localScale = new Vector3(1f, 1.5f, 1f);
    }

    public void ResetTabScale()
    {
        background.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
