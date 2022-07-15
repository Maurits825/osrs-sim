using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public CombatController combatController;
    public Vector3Int CurrentTile { get; private set; }
    public enum States
    {
        Idle = 0,
        MovingToTarget,
        AttackingTarget,
    }

    public States currentState = States.Idle;

    private PathFinder pathFinder;

    public virtual void OnGameTick()
    {
        switch (currentState)
        {
            case States.Idle:
                break;

            case States.MovingToTarget:
                FindPath();
                break;

            case States.AttackingTarget:
                break;

            default:
                break;
        }
    }

    protected virtual void Start()
    {
        Debug.Log("Register: " + stats.enemyName.ToString());
        EnemyController.Instance.RegisterEnemy(this);

        if (stats.isAggresive)
        {
            currentState = States.MovingToTarget;
        }

        pathFinder = new PathFinder();
    }

    public void DealDamage(int amount)
    {
        stats.health -= amount;
        currentState = States.MovingToTarget;
    }

    private void FindPath()
    {
        //List<Vector3Int> path = pathFinder.FindPath(CurrentTile, PlayerController);
    }
}
