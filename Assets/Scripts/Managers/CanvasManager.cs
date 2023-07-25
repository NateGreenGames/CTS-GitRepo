using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public CanvasHUD canvasHUD;
    public CanvasFE canvasFE;
    public void ShowCanvasHUD()
    {
        canvasHUD = Instantiate(Resources.Load("Canvas/" + "CanvasHUD") as 
            GameObject).GetComponent<CanvasHUD>();
    }

    public void ShowCanvasFE()
    {
        Instantiate(Resources.Load("Canvas/" + "CanvasFE") as 
            GameObject).GetComponent<CanvasFE>();
    }
}
