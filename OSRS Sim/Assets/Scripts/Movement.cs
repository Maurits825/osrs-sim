using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public RunEnergy runEnergy;

    public const int RUN_ENERGY_MAX = 100;
    private float runEnergyAmount;
    private float runEnergyRegen;

    private List<GameObject> pooledTileMarker;
    private List<GameObject> currentPathTiles = new List<GameObject>();
    [SerializeField] private GameObject pathTileMarker;

    private PathFinder pathFinder;

    private void Start()
    {
        runEnergyAmount = 0;
        runEnergyRegen = 10f;

        CreateTileMarkerPool();

        pathFinder = new PathFinder();
    }

    private void Update()
    {
        runEnergyAmount += runEnergyRegen * Time.deltaTime;
        runEnergyAmount = Mathf.Clamp(runEnergyAmount, 0f, RUN_ENERGY_MAX);

        runEnergy.SetRunEnergy(GetRunEnergyNormalized());
    }

    public void TrySpendEnergy(int amount)
    {
        if (runEnergyAmount >= amount)
        {
            runEnergyAmount -= amount;
        }
    }

    public Vector3Int ProcessMovement(Vector3Int player, Vector3Int target)
    {
        ClearCurrentPathTiles();
        List<Vector3Int> path = pathFinder.FindPath(player, target);
        DrawPath(path);

        //TODO isRun boolean --> index is then 0 or 2?
        //drain run energy here? runEnergyAmount += runEnergyRegen * Time.deltaTime; based on running or not
        //update ui
        return path[1];
    }

    private float GetRunEnergyNormalized()
    {
        return runEnergyAmount / RUN_ENERGY_MAX;
    }

    private void DrawPath(List<Vector3Int> path)
    {
        foreach (Vector3Int tile in path.GetRange(1, path.Count - 1))
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
