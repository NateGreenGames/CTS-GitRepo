using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class FlaregunBehavior : MonoBehaviour
{
   // public bool isLoaded;
    public float reloadTime;
    public Transform flareSpawnPos;
    public GameObject flarePrefab;
    public ItemSwapping itemSwapping;
    public Camera cam; //Variable to get the camera for the projectile shooting function
    public float projectileSpeed = 30; //Changeable to determine how fast the flare shoots
    public bool debugFlare; //Bool to allow debugging and testing with infinite flares.
    public int flaresLeft;
    public bool enableRumble;

    private Vector3 destination; //Variable that stores the vector3 for the destination of the projectile based on the raycast
    public Inventory playerInventory;
    public ItemData itemData;
    public bool isReloading;
    [Tooltip("Enables controller rumble on flaregun operation")]
    private void Awake()
    {
        playerInventory = GameManager.gm.playerReference.GetComponent<Inventory>();
        cam = Camera.main;
        debugFlare = false;
        itemData = GameManager.gm.itemDatas[1];
        itemSwapping = GameManager.gm.playerReference.GetComponentInChildren<ItemSwapping>(true);
    }
  
    public void FlareGunOperation()
    {
        // Shoots the flaregun if isEquipped bool is true from soFlareGun as well if the gun is loaded.
        if (itemSwapping.FlareGunEquipped() && GameManager.gm.playerProgressionUnlock.flareGunLoaded == true)
        {
            if (GameManager.gm.playerProgressionUnlock.flareGunLoaded == false) Debug.Log("Reload Flare Gun!");
            else Debug.Log("Flare Gun fired");

            OnFirePressed();
            GameManager.gm.playerProgressionUnlock.flareGunLoaded = false;
        }
       
        // TODO Wire in UI element to show remaining flares/some sort of sound cue for dry firing.
   
    }

    public void Reload()
    {
        //If you have flares and hit the R key it will set the isLoaded variable back to loaded.
        InventoryItem flares = null;
        foreach (InventoryItem item in playerInventory.inventory)
        {
            if (item.itemData.displayName == "Flare")
            {
                flares = item;
            }
        }
        if (flares != null)
        {
            flaresLeft = flares.stackSize;
            Debug.Log($"Flare has {flaresLeft} stacks left");
        }

        if (GameManager.gm.playerProgressionUnlock.curFlares >= 0)
        {
            //If you try to reload without flares in inventory, it will not allow you to reload.
            if (GameManager.gm.playerProgressionUnlock.curFlares == 0)
            {
                Debug.Log("No more flares");
                //TODO Wire in UI element to show remaining flares/some sort of sound cue for dry firing.
                return;
            }
            else
            {
                isReloading = true;
                StartCoroutine(ReloadCoroutine());
                GameManager.gm.playerProgressionUnlock.curFlares--;
                playerInventory.Remove(itemData);
            }
        }
    }


    private IEnumerator ReloadCoroutine()
    {
        //Play reload animation
        yield return new WaitForSeconds(reloadTime);
        GameManager.gm.playerProgressionUnlock.flareGunLoaded = true;
        isReloading = false;
    }
    private void OnFirePressed()
    {
        if (GameManager.gm.playerProgressionUnlock.flareGunLoaded)
        {
            ShootProjectile();
            if (enableRumble) StartCoroutine(RumblePulse());
        }
    }
   /* private void RayCastFiring()
    {
        Vector3 projectileOrigin = flareSpawnPos.transform.position;

        Vector3 targetPosition = gameObject.transform.forward;
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f), 0);

        RaycastHit hit;

        if (Physics.Raycast(cameraPosition, Camera.main.transform.forward, out hit))
        {
            targetPosition = hit.collider.ClosestPoint(cameraPosition);
        }
        Vector3 projectileVector = targetPosition - projectileOrigin;
        if (isLoaded)
        {
            FlareBehavior flare = Instantiate(flarePrefab, flareSpawnPos.position, Quaternion.identity).GetComponent<FlareBehavior>();
            flare.SetFlareTrajectory(projectileVector.normalized);
        }
    } */

    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }
        InstantiateProjectile();
    }
    void InstantiateProjectile()
    {
        FlareBehavior flare = Instantiate(flarePrefab, flareSpawnPos.position, Quaternion.identity).GetComponent<FlareBehavior>();
        flare.GetComponent<Rigidbody>().velocity = (destination - flareSpawnPos.position).normalized * projectileSpeed;
    }

    IEnumerator RumblePulse()
    {
        Gamepad.current.SetMotorSpeeds(0.75f, 0.75f);
        yield return new WaitForSeconds(.2f);
        Gamepad.current.ResetHaptics();
    }
}