using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "NewNpcInfo")]
public class NpcInfo : ScriptableObject
{
    public NpcStats npcStats;

    public string npcName;

    public bool canMove;
    public int moveSpeed;
    public bool isAggresive;

    public int attackRange;
    public int maxHit; //todo diff max hits with diff styles...

    public int attackSpeed;

    public Vector2Int size;
}

