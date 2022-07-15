using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;

    public int maxHealth;
    [NonSerialized]
    public int health;

    public bool canMove;
    public bool isAggresive;

    public void OnAfterDeserialize()
    {
        health = maxHealth;
    }
}
