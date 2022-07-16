using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "NewNpcStats")]
public class NpcStats : ScriptableObject
{
    //combat stats
    public NpcStatsValue health;
    public NpcStatsValue attack;
    public NpcStatsValue strength;
    public NpcStatsValue defence;
}

