using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sChest : MonoBehaviour
{
    public GameObject hatch;

    public void OpenChest()
    {
        hatch.transform.Rotate(-60.0f, 0.0f, 0.0f);
    }
}
