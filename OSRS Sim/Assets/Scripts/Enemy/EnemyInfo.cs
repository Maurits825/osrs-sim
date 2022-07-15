using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public string enemyName;

    public int maxHealth;
    public int health;

    public bool canMove;
    public int moveSpeed;
    public bool isAggresive;

    public int attackRange;
    public int maxHit;

    public Vector3Int currentTile;

    public void OnEnable()
    {
        health = maxHealth;
    }
}
