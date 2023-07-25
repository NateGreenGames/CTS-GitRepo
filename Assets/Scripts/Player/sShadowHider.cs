using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sShadowHider : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;
    private void Start()
    {
        ShadowsOnly();
    }


    public void ShadowsOnly()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
        
    }

    public void ShadowsAndBody()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

}
