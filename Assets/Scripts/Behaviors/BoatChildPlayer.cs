using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatChildPlayer : MonoBehaviour
{
    GameManager gm;
    GameObject player;
    private void Start()
    {
        gm = GameManager.gm;
        player = gm.playerReference;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) player.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            transform.DetachChildren();
            player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);
        }
    }
}
