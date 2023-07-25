using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{ 
    public void takeDamage(int _damageTaken);
    public void onDeath();
}
