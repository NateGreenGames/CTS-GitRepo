using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstrolabeBehavior : MonoBehaviour
{
    public GameObject animatedDial, programmedDial;
    [ReadOnly][SerializeField] Vector3 waypoint;

    private GameObject astrolabeMain;
    private Animator m_Anim;
    private bool objectiveSeekingIsPaused = false;

    private void Start()
    {
        astrolabeMain = programmedDial.transform.parent.gameObject; //Stinky hack to get the parent of the parent of the dial, don't @ me, don't rearange the hierarchy or this will cause problems.
        m_Anim = astrolabeMain.GetComponent<Animator>();
        animatedDial.SetActive(false);
        ChangeHeading(GameManager.gm.boatReference.transform.position);
    }

    void Update()
    {
        if (!objectiveSeekingIsPaused)
        {
            UpdateDialFacing();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !objectiveSeekingIsPaused)
        {
            StartCoroutine(FlourishRoutine());
        }
    }
    
    public IEnumerator FlourishRoutine()
    {
        ToggleDialVisibility();
        objectiveSeekingIsPaused = true;
        m_Anim.SetTrigger("OnFlourish");
        yield return new WaitForSeconds(2.6f);
        objectiveSeekingIsPaused = false;
        ToggleDialVisibility();
    }

    public void ChangeHeading(Vector3 _newWaypoint)
    {
        StartCoroutine(FlourishRoutine());
        waypoint = _newWaypoint;
    }
    private void ToggleDialVisibility()
    {
        animatedDial.SetActive(!animatedDial.activeInHierarchy);
        programmedDial.SetActive(!animatedDial.activeInHierarchy);
    }
    private void UpdateDialFacing()
    {
        Vector3 target = waypoint; //Update this when the quest system has it's waypoints exposed.
        Vector3 relativeTarget = GameManager.gm.playerReference.transform.InverseTransformPoint(target);
        float needleRotation = Mathf.Atan2(relativeTarget.z, -relativeTarget.x) * Mathf.Rad2Deg;
        programmedDial.transform.localRotation = Quaternion.Euler(-90, needleRotation, 0);
    }
}
