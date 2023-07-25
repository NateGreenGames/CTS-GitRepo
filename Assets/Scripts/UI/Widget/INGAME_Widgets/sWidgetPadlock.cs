using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sWidgetPadlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passFailText;
    [SerializeField] private float timeToWaitAfterSolution;
    [SerializeField] private TMP_Dropdown[] tumblerReferences = new TMP_Dropdown[3];
    private int[] tumblerCombination;
    private sPadlock m_Padlock;


    private void Start()
    { 
        passFailText.text = "";
    }

    public void Init(int[] _lockCombination, sPadlock lockedObject)
    {
        tumblerCombination = _lockCombination;
        m_Padlock = lockedObject;
    }
    public void OnCheckClick()
    {
        if (tumblerReferences[0].value == (tumblerCombination[0]) && tumblerReferences[1].value == tumblerCombination[1] && tumblerReferences[2].value == tumblerCombination[2])
        {
            passFailText.text = "Correct!";
            StartCoroutine(CorrectClose());
        }
        else
        {
            passFailText.text = "Wrong.";
        }
    }
    public void OnBackClick()
    {
        GameManager.gm.playerReference.GetComponent<PlayerMovement>().movementEnabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Destroy(this.gameObject);
    }

    IEnumerator CorrectClose()
    {
        yield return new WaitForSeconds(timeToWaitAfterSolution);
        OnBackClick();
        m_Padlock.Solved();
    }
}
