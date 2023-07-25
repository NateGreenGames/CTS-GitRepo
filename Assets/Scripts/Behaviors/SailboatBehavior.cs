using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SailboatBehavior : MonoBehaviour
{
    [Header("Boat Controller:")]
    [SerializeField] private Animator m_Anim;
    private Rigidbody m_RB;
    private float crestWindspeed;
    private float crestWindAngle;
    private float boatAngle;
    public float sailRotationDirection = 0;
    public float verticalInput = 0;

    [Range(0,1)] [SerializeField] private float againstWindSpeed;
    [SerializeField] private float overallSpeedMultiplier;
    [Range(0, 100)] public float sailLetOutAmount;
    [SerializeField] private float sailMovementSpeed;
    [Range(-90, 90)] public float tillerAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float tillerRecenterSpeed;
    [Range(-90, 90)] [SerializeField] private float sailAngle;
    [SerializeField] private float sailRotationSpeed;

    [Space]
    [Header("Cameras:")]
    public GameObject thirdPersonCam;
    [SerializeField] private GameObject firstPersonCam;
    [SerializeField] public bool fpsCamOn;
    [SerializeField] public bool tpsCamOn;
    public bool flareOnCooldown;

    [Space]
    [Header("Debug:")]
    [SerializeField] private bool postDebugInformation = false;
    [SerializeField] private float autoFurlDuration;
    private HesseSailingAnimManager hesseSailingAnimManager;
    private ControllerManager controllerManager;
    private GameManager gm;

    [SerializeField] GameObject tillerGameObject;
    [SerializeField] float maxAngleStarboard;
    [SerializeField] float maxAnglePort;

    private void Start()
    {
        gm = GameManager.gm;
        AnimationUpdater();
        m_RB = gameObject.GetComponent<Rigidbody>();
        thirdPersonCam.SetActive(false);
        firstPersonCam.SetActive(false);
        tpsCamOn = true;
        flareOnCooldown = false;
        this.enabled = false;

        hesseSailingAnimManager = GetComponentInChildren<HesseSailingAnimManager>();
        controllerManager = gm.boatReference.GetComponent<ControllerManager>();

        #region Frontend Boat controls
        if (GameManager.gm.curScene == GameManager.eScene.CTS_FrontEndScene)
        {
            this.enabled = true;
            sailLetOutAmount = 100;
            sailAngle = -16;
        }
        else
        {
            sailLetOutAmount = 0;
            sailAngle = 0;
            this.enabled = false;
        }
        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        //Correcting wind speed and direction every frame.
        crestWindspeed = gm.crestReference.wavesReference._windSpeed;
        crestWindAngle = gm.crestReference.wavesReference._waveDirectionHeadingAngle;

        RaiseOrLowerSails();
        RotateBoat();
        RotateSail();
        AnimationUpdater();
    }

    private void FixedUpdate()
    {
        //Propelling the ship forward based on how perpindicular to the wind the sail is, the amount the sail has been let out, and the speed of the wind itself.
        m_RB.AddForce(transform.forward * CheckWindPropulsion() * crestWindspeed * sailLetOutAmount * overallSpeedMultiplier);
    }

    private void AnimationUpdater()
    {
        m_Anim.SetFloat("Sail Blend", Mathf.InverseLerp(-90, 90, sailAngle));
        tillerGameObject.transform.localRotation = Quaternion.Euler(-90, 0, Mathf.Lerp(maxAngleStarboard, maxAnglePort, Mathf.InverseLerp(90, -90, tillerAngle)));
        m_Anim.SetFloat("Rutter Blend", Mathf.InverseLerp(90, -90, tillerAngle));
        m_Anim.SetFloat("Sail Let Out", Mathf.InverseLerp(0, 100, sailLetOutAmount));
        m_Anim.SetFloat("Sail Billow Blend", Mathf.InverseLerp(-90, 90, sailAngle));
    }
    private float CheckWindPropulsion()
    {
        //Gets the current rotation of the boat because the rotation of the sail will be relative to the boat.
        boatAngle = gameObject.transform.rotation.eulerAngles.y;

        //Converting the angle of the wind and the angle of the sail on the boat into vectors so they can be used to judge how perpindicular they are to eachother.
        Vector2 windHeadingVector = new Vector2(Mathf.Cos(crestWindAngle * Mathf.Deg2Rad), Mathf.Sin(crestWindAngle * Mathf.Deg2Rad));
        Vector2 sailFacingVector = new Vector2(Mathf.Cos((sailAngle + boatAngle) * Mathf.Deg2Rad), Mathf.Sin((sailAngle + boatAngle) * Mathf.Deg2Rad));

        //Normalizing the vectors so we get an answer between 0 and 1
        windHeadingVector = windHeadingVector.normalized;
        sailFacingVector = sailFacingVector.normalized;

        //Get the dot product of these normalized vectors, the closer it is to 0, the closer these vectors are to being perpindicular to eachother.
        float nearnessToPerpendicular = (windHeadingVector.x * sailFacingVector.x) + (windHeadingVector.y * sailFacingVector.y);

        if(postDebugInformation)Debug.Log(nearnessToPerpendicular);

        if(nearnessToPerpendicular < againstWindSpeed)
        {
            if (postDebugInformation) Debug.Log($"Boat Output: {againstWindSpeed}.");
            return againstWindSpeed;
        }
        else
        {
            if (postDebugInformation) Debug.Log($"Boat Output: {nearnessToPerpendicular}.");
            return nearnessToPerpendicular;
        }
    }
    #region Raising and lowering sails
    private void RaiseOrLowerSails()
    {
        if(verticalInput != 0)
        {
            sailLetOutAmount += verticalInput * sailMovementSpeed * Time.deltaTime;
            sailLetOutAmount = Mathf.Clamp(sailLetOutAmount, 0, 100);
        }
        else
        {
            return;
        }
    }

    public IEnumerator FurlSails()
    {
        float elaspedTime = 0;
        while (!controllerManager.isControllingBoat && elaspedTime < autoFurlDuration && sailLetOutAmount != 0)
        {
            sailLetOutAmount = Mathf.Clamp(Mathf.Lerp(sailLetOutAmount, 0, Mathf.InverseLerp(0, autoFurlDuration, elaspedTime)), 0, 100);
            tillerAngle = 0;
            // Debug.Log(sailLetOutAmount);
            yield return new WaitForEndOfFrame();
            elaspedTime += Time.deltaTime;
            AnimationUpdater();
            if (sailLetOutAmount < 0.1f) sailLetOutAmount = 0;
        }
    }

    public void IncreaseSail()
    {
        verticalInput++;
    }

    public void DecreaseSail()
    {
        verticalInput--;
    }
    public void ResetSail()
    {
        verticalInput = 0;
    }

    #endregion
    #region rotating boat code
    private void RotateBoat()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return;
        }
        float horizontalInput = gamepad.leftStick.ReadValue().x;
        if (horizontalInput != 0)
        {
            tillerAngle += horizontalInput;
            tillerAngle = Mathf.Clamp(tillerAngle, -90, 90);
        }

        hesseSailingAnimManager.hesseSailAnim.SetFloat("Blend", Mathf.InverseLerp(-90, 90, tillerAngle)); 

        if (horizontalInput == 0 && tillerAngle != 0)
        {
            if(tillerAngle > 0)
            {
                tillerAngle -= tillerRecenterSpeed;
            }
            else
            {
                tillerAngle += tillerRecenterSpeed;
            }
        }
        gameObject.transform.Rotate(Vector3.up * tillerAngle * rotationSpeed * Time.deltaTime);
    }
    #endregion
    #region Rotating Sail Code
    private void RotateSail()
    {
        if(sailRotationDirection != 0)
        {
            sailAngle += sailRotationDirection * sailRotationSpeed * Time.deltaTime;
            sailAngle = Mathf.Clamp(sailAngle, -90, 90);
        }
    }
    
    public void RotateSailRight()
    {
        sailRotationDirection = sailRotationDirection + 3.75f;
    }

    public void RotateSailLeft()
    {
        sailRotationDirection = sailRotationDirection - 3.75f;
    }

    public void ResetSailRotation()
    {
        sailRotationDirection = 0;
    }
    #endregion
    public void SwitchToFirst()
    {
        thirdPersonCam.SetActive(false);
        firstPersonCam.SetActive(true);
        tpsCamOn = false;
        fpsCamOn = true;
        gm.playerReference.GetComponent<sShadowHider>().ShadowsOnly();
    }
    public void SwitchToThird()
    {
        thirdPersonCam.SetActive(true);
        firstPersonCam.SetActive(false);
        tpsCamOn = true;
        fpsCamOn = false;
        gm.playerReference.GetComponent<sShadowHider>().ShadowsAndBody();
    }

    public void SwitchCams()
    {
        if (thirdPersonCam.activeInHierarchy == true) SwitchToFirst();
        else SwitchToThird();
    }

    public void FireFlare()
    {
        if (flareOnCooldown != true && gm.boatReference.GetComponent<SailboatBehavior>().enabled == true)
        {
            flareOnCooldown = true;
            GetComponentInChildren<sBoatFlare>().ShootBoatFlare();
            StartCoroutine(FlareCooldown());
        }
    }

    IEnumerator FlareCooldown()
    {
        gm.canvasManager.canvasHUD.tutorialHUD.shipFlarePrompt.SetActive(false);
        yield return new WaitForSeconds(30);
        gm.canvasManager.canvasHUD.tutorialHUD.shipFlarePrompt.SetActive(true);
        flareOnCooldown = false;
    }

}
