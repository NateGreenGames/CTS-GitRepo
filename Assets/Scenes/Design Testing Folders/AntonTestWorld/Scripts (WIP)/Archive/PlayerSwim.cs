using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PlayerSwim : MonoBehaviour
{
    public bool inWater = false;

    private InputManager inputManager;
    private PlayerMovement playerMovement;

    [SerializeField]
    private Camera pCamera;
    private float waterSurfacePosition = 0.0f;
    private Transform waterSurface;

    private AudioSource swimmingAudio;

    [Header("Movement")]

    public float Gravity = -2f;
    public float Damping = 1f;

    [Header("Speed")]
    public float Acceleration = 1f;
    public float NormalSpeed = 2f;
    public float FastSpeed = 4f;
    public float SidewaysSpeed = 1.5f;
    public float BackSpeed = 1f;
    public float UpSpeed = 10f ;

    [Header("Sounds")]
    [Tooltip("Sounds that will play when entering water")]
    public AudioClip EnterWater;

    [Tooltip("Sounds that will play when exiting water")]
    public AudioClip ExitWater;

    [Tooltip("Sounds that will play when swimming underwater")]
    public AudioClip SwimmingUnderWater;

    [Header("Effects")]
    public Color WaterFogColor;
    public float FogDensity = 0.25f;
    public PostProcessProfile UnderwaterPostProcessing;

    private bool defFogEnabled;
    private Color defFogColor;
    private FogMode defFogMode;
    private float defFogDensity;


    // Start is called before the first frame update
    void Start()
    {
        // Grabs the controller from the player
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<InputManager>();
        swimmingAudio = GetComponent<AudioSource>();

        // Sets the Fog
        defFogEnabled = RenderSettings.fog;
        defFogColor = RenderSettings.fogColor;
        defFogMode = RenderSettings.fogMode;
        defFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        // if player is swimming then update swimming
        if (playerMovement.isSwimming)
        {
            UpdateSwimming();
        }
    }
    private void UpdateSwimming()
    {
        if (playerMovement.isSwimming)
        {
            if (!swimmingAudio.isPlaying && SwimmingUnderWater != null && inWater && IsUnderwater())
            {
                swimmingAudio.clip = SwimmingUnderWater;
                swimmingAudio.Play();
            }
          
            SwimSurface();
        }
    }

    public void WaterEffects(bool Active)
    {
        // Makes some pretty effects
        if (Active)
        {
            // cameraplay raindrop_off(1);
            RenderSettings.fog = true;
            RenderSettings.fogColor = WaterFogColor;
            RenderSettings.fogMode =  FogMode.Exponential;
            RenderSettings.fogDensity = FogDensity;
        }
        else
        {
            RenderSettings.fog = defFogEnabled;
            RenderSettings.fogColor = defFogColor;
            RenderSettings.fogMode = defFogMode;
            RenderSettings.fogDensity = defFogDensity;
        }
    }

    // If our character is underwater
    public bool IsUnderwater()
    {
        return pCamera.gameObject.transform.position.y < waterSurfacePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure that the layer is water
        if (LayerMask.LayerToName(other.gameObject.layer) == "Water" && !IsUnderwater())
        {
            // Checks if the player is in water and then sets the speed values to water speed values
            inWater = IsInWater(true);
            // Set the water surface to be the position of the collider we enter
            waterSurface = other.transform;
            waterSurfacePosition = waterSurface.position.y;
            playerMovement.isSwimming = true;
            StartCoroutine(EnteredWater());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Make sure that the layer is water
        if (LayerMask.LayerToName(other.gameObject.layer) == "Water" && !IsUnderwater())
        {
            // Sets the player to be not in water
            inWater = IsInWater(false);
            waterSurfacePosition = 0f;

            if (swimmingAudio)
            {
                swimmingAudio.Stop();
            }
            playerMovement.isSwimming = false;
            StopCoroutine(EnteredWater());
        }
    }

    public void SwimSurface()
    {
        if (IsUnderwater())
        {
            // Plays camera droplet water effect
            WaterEffects(true);
        }
        else if (!IsUnderwater())
        {
            // camera droplet effect lasts (3) seconds
            if (ExitWater != null && swimmingAudio.clip == SwimmingUnderWater)
            {
                swimmingAudio.clip = ExitWater;
                swimmingAudio.Play();
            }
            WaterEffects(false);
        }
    }

    public bool IsInWater(bool inWater)
    {
        if (inWater)
        {
            playerMovement.speed = NormalSpeed;
            playerMovement.sprintMod = .75f;
            playerMovement.gravity = Gravity;

            // Play sound effect as we enter the water
            if (EnterWater != null && playerMovement.isSwimming)
            {
                swimmingAudio.clip = EnterWater;
                swimmingAudio.Play();
            }

            Debug.Log($"Water Tigger Enter: {inWater}");
            return true;
        }
        else
        {
            playerMovement.speed = 5.0f;
            playerMovement.sprintMod = 1.5f;
            playerMovement.gravity = -9.8f;

            // Camera.Play.RainDrop_OFF(2);
            if (swimmingAudio)
            {
                swimmingAudio.Stop();
            }
            if (ExitWater != null && swimmingAudio)
            {
                swimmingAudio.clip = ExitWater;
                swimmingAudio.Play();
            }
            Debug.Log($"Water Tigger Enter: {inWater}");
            return false;
        }

        
    }
    public void On_SwimUp()
    {
        if (inWater)
        {
            playerMovement.playerVelocity += Vector3.up * UpSpeed * Time.deltaTime;
        }
    }

    public IEnumerator EnteredWater()
    {
        yield return StartCoroutine(WaitFor.Frames(10)); // wait for 10 frames
        On_SwimUp();
    }
}

public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
