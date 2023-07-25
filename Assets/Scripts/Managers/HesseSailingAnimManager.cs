using UnityEngine;

public class HesseSailingAnimManager : MonoBehaviour
{
    public Animator hesseSailAnim;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        hesseSailAnim = GetComponent<Animator>();
        gm = GameManager.gm;
    }

    public void HandleHesseAnimation(bool isOperatingBoat)
    {
        // Animations for if the player is sitting down or standing up from controlling the boat
        if (isOperatingBoat)
        {
            hesseSailAnim.SetBool("isControllingBoat", true);
            gm.playerReference.GetComponent<PlayerMovement>().hessePlayerModel.SetActive(false);
            gm.playerReference.GetComponent<PlayerMovement>().hesseHandsModel.SetActive(false);

        }
        else if (!isOperatingBoat)
        {
            hesseSailAnim.SetBool("isControllingBoat", false);
            gm.playerReference.GetComponent<PlayerMovement>().hessePlayerModel.SetActive(true);
            gm.playerReference.GetComponent<PlayerMovement>().hesseHandsModel.SetActive(true);

        }
    }
}
