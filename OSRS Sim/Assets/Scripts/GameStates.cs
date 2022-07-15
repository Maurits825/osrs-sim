using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "GameState")]
public class GameStates : ScriptableObject
{
    public enum States
    {
        Idle = 0,
        TileClicked,
        Moving,
        EnemyClicked,
        MovingToEnemy,
        Attacking,
    }

    public States currentState;

    public float tickLength = 0.6f;

    public int startTick = 0;
    [NonSerialized]
    public int currentTick;

    public void OnAfterDeserialize()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        currentTick = startTick;
        currentState = States.Idle;
    }
}

