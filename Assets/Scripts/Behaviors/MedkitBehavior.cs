using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitBehavior : MonoBehaviour, IInteractable
{
    public int healAmount = 25;
    public bool destroyOnUse;
    public string HudMessage { get; set; }
    public bool isInteractable { get; set; }

    private PlayerMovement playerRef;

    private void Start()
    {
        isInteractable = true;
        playerRef = GameManager.gm.playerReference.GetComponent<PlayerMovement>();
    }

    public void OnLookingAt()
    {
        if (playerRef.currentHealth < playerRef.startingHealth) HudMessage = "Press E to heal";
        else HudMessage = "Health is full";
    }
    public void OnInteract()
    {
        if (playerRef.currentHealth < playerRef.startingHealth) playerRef.currentHealth += healAmount;
        else
            return;

        if (playerRef.currentHealth > playerRef.startingHealth) playerRef.currentHealth = playerRef.startingHealth;

        if (destroyOnUse) Destroy(gameObject);
    }
}
