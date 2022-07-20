using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour, IMovement
{
    private Npc npc;

    private Vector2Int targetTile;
    private Npc npcTarget;

    private int roamRange = 5;// TODO put in npcInfo?
    private PathFinder pathFinder;

    public void Move()
    {
        List<Vector2Int> path = pathFinder.FindPath(npc.currentTile, targetTile); //TODO npc path finder
        
        if (path.Count <= npc.npcInfo.moveSpeed)
        {
            npc.currentTile = path[^1];
        }
        else
        {
            npc.currentTile = path[npc.npcInfo.moveSpeed];
        }
    }

    public void OnGameTick()
    {
        switch (npc.npcStates.currentState)
        {
            case NpcStates.States.Idle:
                npc.npcStates.nextState = Roam();
                break;

            case NpcStates.States.Moving:
                Move();
                if (npc.currentTile == targetTile)
                {
                    npc.npcStates.nextState = NpcStates.States.Idle;
                }
                else
                {
                    npc.npcStates.nextState = NpcStates.States.Moving;
                }
                break;

            case NpcStates.States.MovingToNpc: //TODO copy pasted from playermovement
                if (npcTarget.IsInRange(npc.currentTile, npc.npcInfo.attackRange))
                {
                    npc.npcStates.nextState = NpcStates.States.AttackingNpc;
                }
                else
                {
                    targetTile = npcTarget.GetClosestAdjacentTile(npc.currentTile);
                    Move();

                    if (npcTarget.IsInRange(npc.currentTile, npc.npcInfo.attackRange))
                    {
                        npc.npcStates.nextState = NpcStates.States.AttackingNpc;
                    }
                    else
                    {
                        npc.npcStates.nextState = NpcStates.States.MovingToNpc;
                    }
                }
                break;
        }
    }

    public void SetTargetNpc(Npc target)
    {
        npcTarget = target;
    }

    private void Start()
    {
        npc = GetComponent<Npc>();
        pathFinder = new PathFinder();
    }

    private NpcStates.States Roam() //TODO if target tile is unwalkable, it will forever be in moving state?
    {
        if (Random.Range(0, 11) < 3)
        {
            int randX = Random.Range(-roamRange, roamRange);
            int randY = Random.Range(-roamRange, roamRange);
            targetTile = new Vector2Int(
                npc.spawnTile.x + randX,
                npc.spawnTile.y + randY);

            return NpcStates.States.Moving;
        }
        else
        {
            return NpcStates.States.Idle;
        }
    }

    public void SetTargetTile(Vector2Int target)
    {
        targetTile = target;
    }

    public Vector2Int GetTargetTile()
    {
        return targetTile;
    }
}
