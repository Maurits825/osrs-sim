using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcState", menuName = "NpcState")]
public class NpcStates : ScriptableObject
{
    public enum States
    {
        Idle = 0,
        Moving,
        MovingToNpc,
        AttackingNpc,
    }

    public States currentState;
    public States nextState;
}

