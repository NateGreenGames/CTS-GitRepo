using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TabArea : MonoBehaviour
{
    public List<TabButtons> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButtons selectedTab;
    public List<GameObject> objectsToSwap;
    private int idx;
    private GameObject[] tabs;

    [SerializeField] AudioClip clip;
    public AudioSource audioSource;

    public void Subscribe(TabButtons button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtons>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtons button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();
        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtons button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }

    public void PlayButtonSFX()
    {
        audioSource.PlayOneShot(clip);
    }
}
