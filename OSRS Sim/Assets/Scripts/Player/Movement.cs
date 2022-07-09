using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3Int CurrentPlayerTile { get; private set; }
    [SerializeField] private PlayerVariables playerVariables;
    private float runEnergyRegen;

    private List<GameObject> pooledTileMarker;
    private List<GameObject> currentPathTiles = new List<GameObject>();
    [SerializeField] private GameObject pathTileMarker;

    private PathFinder pathFinder;

    private GameObject playerTileMarker;
    private void Start()
    {
        playerVariables.RunEnergy = 0;
        runEnergyRegen = 10f;

        CreateTileMarkerPool();

        pathFinder = new PathFinder();

        //TODO when game start set currentplayertile?
        playerTileMarker = GetPooledTileMarker();
    }

    public Vector3Int? ProcessMovement(Vector3Int target)
    {
        ClearCurrentPathTiles();

        playerVariables.RunEnergy += runEnergyRegen;
        
        List<Vector3Int> path = pathFinder.FindPath(CurrentPlayerTile, target);

        if (path.Count == 0 || path[^1] == CurrentPlayerTile)
        {
            return null;
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

        Vector3Int nextTile = path[tileIndex];
        CurrentPlayerTile = nextTile;

        if (Settings.s.drawPlayerTile)
        {
            playerTileMarker.SetActive(true);
            playerTileMarker.transform.position = CurrentPlayerTile;
        }
        else
        {
            playerTileMarker.SetActive(false);
        }

        if (Settings.s.drawPlayerPath)
        {
            DrawPath(path);
        }

        return nextTile;
    }

    private void DrawPath(List<Vector3Int> path)
    {
        int startIndex = 1;
        foreach (Vector3Int tile in path.GetRange(startIndex, path.Count - startIndex))
        {
            GameObject tileObj = GetPooledTileMarker();
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

    private void CreateTileMarkerPool()
    {
        pooledTileMarker = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < 50; i++)
        {
            tmp = Instantiate(pathTileMarker);
            tmp.SetActive(false);
            pooledTileMarker.Add(tmp);
        }
    }

    private GameObject GetPooledTileMarker()
    {
        for (int i = 0; i < 50; i++)
        {
            if (!pooledTileMarker[i].activeInHierarchy)
            {
                return pooledTileMarker[i];
            }
        }
        return null;
    }
}
