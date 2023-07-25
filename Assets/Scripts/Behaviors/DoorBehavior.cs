using System.Collections;
using UnityEngine;

public class DoorBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isHandleOnLeft, isClosed, isLocked;
    public bool isInteractable { get; set; }
    public string HudMessage { get; set; }

    private Animator m_Anim;
    private GameManager gm;
    private InputManager inputManager;
    [SerializeField]
    [Tooltip("Determines if the door/gate is always unlocked from the start")]
    private bool hasBeenUnlocked = false;
    private bool isAnimating;

    private void Start()
    {
        gm = GameManager.gm;
        inputManager = gm.inputManager.GetComponent<InputManager>();
        UpdateHuDText();
        m_Anim = gameObject.GetComponent<Animator>();
        m_Anim.SetBool("IsHandleLeft", isHandleOnLeft);
        m_Anim.SetBool("IsClosed", isClosed);
        isInteractable = true;
    }

    public void OnInteract()
    {
        if (inputManager.onFoot.Interact.triggered)
        {
            if (!isLocked || hasBeenUnlocked)
            {
                if (!isAnimating)
                {
                    isClosed = !isClosed;
                    UpdateHuDText();
                    isAnimating = true;
                    StartCoroutine(TriggerAnimation());
                }
            }
        }
    }
    public void OnLookingAt()
    {
        if (isLocked)
        {
                isLocked = false;
                hasBeenUnlocked = true;
                UpdateHuDText();
                Debug.Log("Gate Unlocked"); // This needs to be changed later to a UI text
        }
    }

    IEnumerator TriggerAnimation()
    {
        m_Anim.SetTrigger("ToggleDoor");
        yield return new WaitForSeconds(.75f);
        isAnimating = false;
    }


    private void UpdateHuDText()
    {
        if (isLocked) HudMessage = "Door Is Locked";
        else
        {
            if (isClosed) HudMessage = "Open Door";
            else HudMessage = "Close Door";
        }
        
    }
}
