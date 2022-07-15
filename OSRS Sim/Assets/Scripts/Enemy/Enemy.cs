using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyInfo defaultEnemyInfo;
    public EnemyInfo enemyInfo;

    public enum States
    {
        Idle = 0,
        MovingToTarget,
        AttackingTarget,
    }

    public States currentState = States.Idle;

    [SerializeField] private CombatController combatController;
    [SerializeField] private PlayerVariables playerVariables;

    private PathFinder pathFinder;

    public virtual void OnGameTick()
    {
        States nextState = currentState;

        switch (currentState)
        {
            case States.Idle:
                break;

            case States.MovingToTarget:
                FindPath();
                if (IsInRange())
                {
                    nextState = States.AttackingTarget;
                }
                break;

            case States.AttackingTarget:
                if (IsInRange())
                {
                    Attack();
                }
                else
                {
                    nextState = States.MovingToTarget;
                }
                break;

            default:
                break;
        }

        currentState = nextState;
    }

    private void Awake()
    {
        enemyInfo = Instantiate(defaultEnemyInfo);
    }

    protected virtual void Start()
    {
        Debug.Log("Register: " + enemyInfo.enemyName.ToString());
        EnemyController.Instance.RegisterEnemy(this);

        if (enemyInfo.isAggresive)
        {
            currentState = States.MovingToTarget;
        }

        pathFinder = new PathFinder();
    }

    public virtual void Damage(int amount)
    {
        enemyInfo.health -= amount;
        EventController.Instance.SpawnHitsplat(amount);

        if (currentState == States.Idle)
        {
            currentState = States.MovingToTarget;
        }
    }

    protected virtual void Attack()
    {
        //TODO make a function later to do dmg math with player def and enemy accuracy
        int damageAmount = Random.Range(0, enemyInfo.maxHit + 1);
        combatController.Damage(damageAmount);
    }

    //TODO will actually need a path finding that is like npc
    private void FindPath()
    {
        List<Vector3Int> path = pathFinder.FindPath(enemyInfo.currentTile, playerVariables.currentTile);
        enemyInfo.currentTile = path[enemyInfo.moveSpeed];
    }

    private bool IsInRange()
    {
        int x = playerVariables.currentTile.x - enemyInfo.currentTile.x;
        int y = playerVariables.currentTile.z - enemyInfo.currentTile.z;
        int range = Mathf.Max(x, y);

        //TODO handle diff enemysizes, walking under?
        return range <= enemyInfo.attackRange;
    }
}
