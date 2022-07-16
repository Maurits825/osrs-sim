using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private PlayerVariables playerVariables;
    [SerializeField] private ObjectPooler tileMarkerPool;

    private List<GameObject> currentPathTiles = new List<GameObject>();

    private PathFinder pathFinder;
    private RunEnergy runEnergy;
    private GameObject playerTileMarker;

    private void Start()
    {
        pathFinder = new PathFinder();
        runEnergy = GetComponent<RunEnergy>();

        tileMarkerPool.CreatePool(); //TODO create the pool somewhere else if other things want to use this pool?

        //TODO when game start set currentplayertile?
        playerTileMarker = tileMarkerPool.GetPooledObject();
    }

    public void OnGameTick()
    {
        runEnergy.RegenRun();
    }

    public Vector3Int ProcessMovement(Vector3Int target)
    {
        ClearCurrentPathTiles();
        
        List<Vector3Int> path = pathFinder.FindPath(playerVariables.currentTile, target);

        if (path.Count <= 1)
        {
            return playerVariables.currentTile;
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

        playerVariables.currentTile = path[tileIndex];

        if (Settings.s.drawPlayerTile)
        {
            playerTileMarker.SetActive(true);
            playerTileMarker.transform.position = playerVariables.currentTile;
        }
        else
        {
            playerTileMarker.SetActive(false);
        }

        if (Settings.s.drawPlayerPath)
        {
            DrawPath(path);
        }

        return playerVariables.currentTile;
    }

    private void DrawPath(List<Vector3Int> path)
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
