using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBase : MonoBehaviour
{
    [SerializeField] bool isMoveDisabled = false;
    //if back button is used when say, a player's move is disabled, check this box to enable it again.

    public void BackButtonPressed() //Removes current menu
    {
        //GameManager.gm.audioManager.ButtonSFX();
        if (isMoveDisabled)
        {
            GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
            GameManager.gm.canvasManager.canvasHUD.hudFunctions.LockMouse();
        }
        if ((int)GameManager.gm.curScene == 0) GameManager.gm.eventSys.firstSelectedGameObject
                = GameManager.gm.canvasManager.canvasFE.newGameBtn;
        Destroy(this.gameObject);
    }
}
