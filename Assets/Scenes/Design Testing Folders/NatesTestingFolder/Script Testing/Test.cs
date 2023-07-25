using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public static UnityAction<int> questEvent;


    private void OnEnable()
    {
        questEvent += test;
    }

    private void OnDisable()
    {
        questEvent -= test;
    }
    private void Start()
    {
        questEvent += test;
    }


    public void test(int _test)
    {

    }
}
