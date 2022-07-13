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

    public States currentState = States.Idle;
}
