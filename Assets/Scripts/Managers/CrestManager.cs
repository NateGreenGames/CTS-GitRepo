using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrestManager : MonoBehaviour
{
    public Crest.ShapeFFT wavesReference;
    public Crest.OceanRenderer oceanRenderer;
    public Crest.OceanDepthCache depthCacheReference;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        wavesReference = gameObject.GetComponentInChildren<Crest.ShapeFFT>();
        oceanRenderer = gameObject.GetComponentInChildren<Crest.OceanRenderer>();
        depthCacheReference = gameObject.GetComponentInChildren<Crest.OceanDepthCache>();
    }

    public void ChangeWindSpeed(float _headingDirectionInDegrees)
    {
        wavesReference._waveDirectionHeadingAngle = _headingDirectionInDegrees;
    }
}
