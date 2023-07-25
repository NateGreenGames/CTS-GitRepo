using UnityEngine;
using UnityEngine.SceneManagement;

public class TillerOutlineBehavior : MonoBehaviour
{
    private SailboatBehavior sailboatBehavior;
    private ControllerManager controllerManager;
    private Outline outline;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CTS_FrontEndScene")
        {
            outline = null;
            sailboatBehavior = null;
            controllerManager = null;
        } 
        else
        {
            outline = GetComponent<Outline>();
            sailboatBehavior = GameManager.gm.boatReference.GetComponent<SailboatBehavior>();
            controllerManager = sailboatBehavior.GetComponent<ControllerManager>();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "CTS_FrontEndScene")
        {
            UpdateOutline();
        }
    }

    private void UpdateOutline()
    {
        if (GameManager.gm.playerReference.transform.parent == null || controllerManager.isControllingBoat) outline.enabled = false;
        else outline.enabled = true;
    }
}
