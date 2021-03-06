using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private ObjectPooler tileMarkerPool;
    [SerializeField] private PlayerVariables playerVariables;

    private Npc npc;
    private Npc npcTarget;
    private PathFinder pathFinder;
    private RunEnergy runEnergy;

    private Vector2Int targetTile;

    private List<GameObject> currentPathTiles = new List<GameObject>();
    private GameObject playerTileMarker;

    public void SetTargetTile(Vector2Int target)
    {
        targetTile = pathFinder.FindClosestWalkableTile(target);
    }

    public Vector2Int GetTargetTile()
    {
        return targetTile;
    }

    public void Move()
    {
        ClearCurrentPathTiles();

        List<Vector2Int> path = pathFinder.FindPath(npc.currentTile, targetTile);

        if (path.Count <= 1)
        {
            return;
        }

        int tileIndex = 1;
        //TODO figure run energy drain maths
        float runEnergyDrain = 0;
        if (playerVariables.isRunning)
        {
            if (path.Count > 2)
            {
                tileIndex = 2;
                runEnergyDrain = 15;
            }
        }

        playerVariables.RunEnergy -= runEnergyDrain;
        if (playerVariables.RunEnergy == 0f)
        {
            playerVariables.isRunning = false;
        }

        npc.currentTile = path[tileIndex];

        if (Settings.s.drawPlayerTile)
        {
            playerTileMarker.SetActive(true);
            playerTileMarker.transform.position = Utils.GetWorldLocation(npc.currentTile);
        }
        else
        {
            playerTileMarker.SetActive(false);
        }

        if (Settings.s.drawPlayerPath)
        {
            DrawPath(path);
        }
    }

    public void OnGameTick()
    {
        runEnergy.RegenRun();

        switch (npc.npcStates.currentState)
        {
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

            case NpcStates.States.MovingToNpc:
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
        runEnergy = GetComponent<RunEnergy>();

        pathFinder = new PathFinder();

        tileMarkerPool.CreatePool();

        playerTileMarker = tileMarkerPool.GetPooledObject();
    }

    private void DrawPath(List<Vector2Int> path)
    {
        int startIndex = 1;
        foreach (Vector3Int tile in path.GetRange(startIndex, path.Count - startIndex))
        {
            GameObject tileObj = tileMarkerPool.GetPooledObject();
            tileObj.transform.position = tile;
            currentPathTiles.Add(tileObj);
            tileObj.SetActive(true);
        }
    }

    private void ClearCurrentPathTiles()
    {
        foreach (GameObject tile in currentPathTiles)
        {
            tile.SetActive(false);
        }

        currentPathTiles.Clear();
    }
}
