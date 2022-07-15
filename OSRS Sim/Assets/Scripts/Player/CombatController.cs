using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Enemy currentEnemy;

    [SerializeField] private PlayerVariables playerVariables;

    public void OnGameTick()
    {
        Debug.Log("Player attacking: " + currentEnemy.enemyInfo.name);
        //TODO dmg type later
        currentEnemy.Damage(10);
    }

    public void Damage(int amount)
    {
        //TODO prayer and many other stuff
        playerVariables.health -= amount;
    }
}
