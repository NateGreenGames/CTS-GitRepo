using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    public float amplitude;
    public float frequency;

    private Vector3 startingScale;

    private void Start()
    {
        startingScale = gameObject.transform.localScale;
    }

    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x + amplitude * Mathf.Sin(frequency * Time.time), transform.localScale.y + amplitude * Mathf.Sin(frequency * Time.time), transform.localScale.z + amplitude * Mathf.Sin(frequency * Time.time));
    }



}
