using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sBoatFlareBehavior : MonoBehaviour
{
    public float duration;
    public float initialForceMultiplier;
    public int hangTime;

    private Vector3 startingTrajectory;
    private Rigidbody m_RB;
    private float timer = 0;
    private void Start()
    {
        m_RB = gameObject.GetComponent<Rigidbody>();
        //m_RB.AddForce(startingTrajectory * initialForceMultiplier, ForceMode.Impulse);
        StartCoroutine(FlareFreeze());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IFlammable burnableObject))
        {
            burnableObject.OnIgnite();
        }
    }
    public void SetFlareTrajectory()
    {
        startingTrajectory = Vector3.left;
    }

    public IEnumerator FlareFreeze()
    {
        yield return new WaitForSeconds(hangTime);
        m_RB.constraints = RigidbodyConstraints.FreezeAll;
    }
}
