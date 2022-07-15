using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Enemy currentEnemy;

    public void OnGameTick()
    {
        Debug.Log("Player attacking: " + currentEnemy.stats.name);
        //TODO dmg type later
        currentEnemy.DealDamage(10);
    }
}
