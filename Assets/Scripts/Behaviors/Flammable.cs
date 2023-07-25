using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour, IFlammable 
{
    public float burnTime = 5f;
    public ItemSwapping itemSwapping;

    private void Start()
    {
        itemSwapping = GameObject.Find("ToolContainer").GetComponent<ItemSwapping>();
    }
    // REMOVE THIS FROM THE PLAYER LATER :D -edit: it's here to stay!!
    public void OnIgnite()
    {
        Instantiate(Resources.Load("Prefabs/Puzzle Components/WildFire", typeof(GameObject)) as GameObject, this.transform);
        WhileBurning();
    }

    public void WhileBurning()
    {
        StartCoroutine(TreeBurn(burnTime));
    }

    IEnumerator TreeBurn(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("TreeBurns in " + time);
        if (gameObject.tag == "Player")
        {
            gameObject.GetComponent<PlayerMovement>().onDeath();
        }
        else 
        Destroy(gameObject);
    }
}
