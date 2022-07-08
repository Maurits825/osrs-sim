using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private PlayerVariables playerVariables;
    private float runEnergyRegen;

    private List<GameObject> pooledTileMarker;
    private List<GameObject> currentPathTiles = new List<GameObject>();
    [SerializeField] private GameObject pathTileMarker;

    private PathFinder pathFinder;

    public Vector3Int CurrentPlayerTile { get; private set; }

    private void Start()
    {
        playerVariables.runEnergy = 0;
        runEnergyRegen = 10f;

        CreateTileMarkerPool();

        pathFinder = new PathFinder();
    }

    private void Update()
    {
        playerVariables.runEnergy += runEnergyRegen * Time.deltaTime;
        playerVariables.runEnergy = Mathf.Clamp(playerVariables.runEnergy, 0f, playerVariables.maxRunEnergy);
    }

    public void TrySpendEnergy(int amount)
    {
        if (playerVariables.runEnergy >= amount)
        {
            playerVariables.runEnergy -= amount;
        }
    }

    public Vector3Int ProcessMovement(Vector3Int target)
    {
        ClearCurrentPathTiles();

        if (target == CurrentPlayerTile)
        {
            return CurrentPlayerTile;
        }
        
        List<Vector3Int> path = pathFinder.FindPath(CurrentPlayerTile, target);

        //drain run energy here? runEnergyAmount += runEnergyRegen * Time.deltaTime; based on running or not

        int tileIndex = 1;
        if (playerVariables.isRunning)
        {
            if (path.Count > 2)
            {
                tileIndex = 2;
            }
        }

        Vector3Int nextTile = path[tileIndex];
        CurrentPlayerTile = nextTile;

        DrawPath(path);

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
