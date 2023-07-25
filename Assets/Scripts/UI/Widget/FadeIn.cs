using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        StartCoroutine(FadeFromLoad());
    }
    public IEnumerator FadeFromLoad()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length +
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log("fade in");
        Destroy(this.gameObject);
    }
}
