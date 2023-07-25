using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggerVolume : MonoBehaviour
{
    [SerializeField] private bool isHidden = true;

    // Start is called before the first frame update
    void Start()
    {
        if (isHidden)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.gm.questManager.localQuestId == 3 || GameManager.gm.questManager.localQuestId == 4)
            GameManager.gm.questManager.AdvanceStep();
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
