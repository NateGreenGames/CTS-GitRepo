using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorderTriggerBox : MonoBehaviour
{
    [SerializeField] private Vector3 worldOrigin;
    [SerializeField] private float worldRadius = 20000, distanceToTeleportPlayer = 1000;
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
            //Run fade out animation on ui.
            Vector3 playerLocation = GameManager.gm.playerReference.transform.position;
            Vector3 origin = worldOrigin;

            Vector3 directionalVector = playerLocation - origin;
            float vectorsMagnitude = directionalVector.magnitude;
            Vector3 directionalUnitVector = directionalVector.normalized;

            Vector3 newTargetPosition = directionalUnitVector * (worldRadius - distanceToTeleportPlayer); ;
            StartCoroutine(MovePlayerRoutine(newTargetPosition));
        }
    }

    private IEnumerator MovePlayerRoutine(Vector3 _PointToMoveBoatTo)
    {
        //Start fade out animation.
        yield return new WaitForSeconds(1); //The number on this line should be equal to the clip length of the fade out animation clip.
        //Move boat to new point.
        GameManager.gm.boatReference.transform.position = _PointToMoveBoatTo;
        //Turn the boat around.
        GameManager.gm.boatReference.transform.Rotate(new Vector3(0, 180, 0));
        //End fade out animation.
    }
}
