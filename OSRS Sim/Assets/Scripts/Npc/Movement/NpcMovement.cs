using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour, IMovement
{
    private Npc npc;

    private Vector3Int target;
    private int roamRange = 5;// TODO put in npcInfo?
    private PathFinder pathFinder;

    public void Move()
    {
        List<Vector3Int> path = pathFinder.FindPath(npc.currentTile, target); //TODO npc path finder
        if (path.Count <= 1)
        {
            if (npc.npcStates.currentState == NpcStates.States.MovingToNpc)
            {
                npc.npcStates.nextState = NpcStates.States.AttackingNpc;
            }
            else
            {
                npc.npcStates.nextState = NpcStates.States.Idle;
            }
            
        }
        else if (path.Count <= npc.npcInfo.moveSpeed)
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
            //we can switch states if idle only?
            case NpcStates.States.Idle:
                npc.npcStates.nextState = Roam();
                break;
        }
    }

    private void Start()
    {
        npc = GetComponent<Npc>();
        pathFinder = new PathFinder();
    }

    private NpcStates.States Roam()
    {
        if (Random.Range(0, 11) < 3)
        {
            int randX = Random.Range(-roamRange, roamRange);
            int randY = Random.Range(-roamRange, roamRange);
            target = new Vector3Int(
                npc.spawnTile.x + randX,
                0,
                npc.spawnTile.z + randY);

            return NpcStates.States.Moving;
        }
        else
        {
            return NpcStates.States.Idle;
        }
    }
}
