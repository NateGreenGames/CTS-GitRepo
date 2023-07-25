using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShowPageDiscoveries : ShowPageBase
{
    public soPageDiscoveries soDiscoveries;

    [Header("Text Input")]
    [SerializeField] private TextMeshProUGUI tHeader;
    [SerializeField] private TextMeshProUGUI tContent;
    [SerializeField] private soPageDiscoveries[] pages;
    public int idx;

    void Start()
    {
        DisplayPage(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Gamepad.current.leftTrigger.wasPressedThisFrame) TurnPageLeft();
        if (Input.GetKeyDown(KeyCode.E) || Gamepad.current.rightTrigger.wasPressedThisFrame) TurnPageRight();
    }

    public void DisplayPage(int _idx)
    {
        _idx = idx;
        tHeader.text = pages[_idx].tHeader;
        tContent.text = pages[_idx].tContent;
    }

    public void TurnPageRight()
    {
        idx++;
        if (idx == pages.Length)
        {
            idx = pages.Length - 1;
        }
        DisplayPage(idx);
        //Debug.Log(idx);
    }

    public void TurnPageLeft()
    {

        if (idx == 0)
        {
            idx = 0;
        }
        else
        {
            idx--;
        }
        DisplayPage(idx);
        //Debug.Log(idx);
    }
}
