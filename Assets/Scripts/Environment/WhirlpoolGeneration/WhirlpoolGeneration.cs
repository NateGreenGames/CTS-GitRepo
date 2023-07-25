using System.Collections;
using UnityEngine;

public class WhirlpoolGeneration : MonoBehaviour
{
    public GameObject whirlpoolPrefab;
    public SphereCollider sphereCollider;
    public InstakillCollider instakillCollider;
    public WhirlpoolCollider whirlpoolCollider;
    public Crest.Whirlpool whirlpool;

    [Header("Player/Boat Game Object")]
    public GameObject player;

    private bool isChangingRadius = false; // Flag to track if radius is currently changing
    private float radiusChangeDuration = 15f; // Duration of the radius change in seconds
    private float timer = 0f; // Timer to track the progress of the radius change
    private int initialMaxRadius = 0; // Initial value of randomMaxRadius
    private int currentRadius;

    // Start is called before the first frame update
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Ship");
        whirlpool = gameObject.GetComponent<Crest.Whirlpool>();
        instakillCollider = GetComponentInChildren<InstakillCollider>();
        whirlpoolCollider = GetComponentInChildren<WhirlpoolCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentRadius = whirlpool._radius;
        if (other.CompareTag("Ship"))
        {
            int newMaxRadius = Random.Range(150, 350);

            if (!isChangingRadius)
            {
                initialMaxRadius = whirlpool._radius;
                isChangingRadius = true;
                Debug.Log($"Whirlpool is increasing in size to {newMaxRadius}");
                StartCoroutine(ChangeRadiusOverTime(initialMaxRadius, newMaxRadius));
            }
            else
            {
                int startRadius = whirlpool._radius;
                StopAllCoroutines();
                isChangingRadius = true;
                StartCoroutine(ChangeRadiusOverTime(startRadius, newMaxRadius));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int startRadius = whirlpool._radius;
        if (other.CompareTag("Ship"))
        {
            if (!isChangingRadius)
            {
                isChangingRadius = true;
                StartCoroutine(ChangeRadiusOverTime(startRadius, 0));
            }
            else
            {
                StopAllCoroutines();
                isChangingRadius = true;
                StartCoroutine(ChangeRadiusOverTime(startRadius, 0));
            }
        }
    }

    private IEnumerator ChangeRadiusOverTime(int startRadius, int targetRadius)
    {
        // Reset the timer
        timer = 0f;

        while (timer < radiusChangeDuration)
        {
            timer += Time.deltaTime;

            // Interpolate the radius between start and target values based on the timer progress
            float t = timer / radiusChangeDuration;
            int lerpedRadius = (int)Mathf.Lerp(startRadius, targetRadius, t);
            SetRadius(lerpedRadius);

            yield return null;

            // Check if the coroutine was stopped prematurely
            //  if (isChangingRadius)   break;

        }

        // Ensure the final radius value is set to the target value if the coroutine completed
        if (isChangingRadius)
        {
            SetRadius(targetRadius);
        }
           
        isChangingRadius = false;
    }

    public void SetRadius(int maxRadius)
    {
        whirlpool._radius = maxRadius;
        whirlpool._amplitude = whirlpool._radius / 6;
        whirlpool._eyeRadius = 0;
        whirlpool._maxSpeed = whirlpool._radius / 12;

        // Calculate the collider radius and center adjustments
        float colliderRadius = maxRadius * 2f / 3f;
        float centerOffset = colliderRadius / 4f;

        float damageColliderRadius = maxRadius * .4f;

        // Update the instakillCollider radius and center
        instakillCollider.SetColliderProperties(colliderRadius, centerOffset);
        whirlpoolCollider.SetDamageColliderProperties(damageColliderRadius);
    }
}
