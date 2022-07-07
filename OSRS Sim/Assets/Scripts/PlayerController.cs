using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private GameObject tileMarker;
    [SerializeField] private Transform player;

    PathNode[,] pathFindingGrid;

    private List<GameObject> pooledTileMarker;
    private List<GameObject> currentPathTiles = new List<GameObject>();

    private TickManager tickManager;
    private PathFinder pathFinder;
    private Vector3Int tileClicked;

    private void Start()
    {
        CreateTileMarkerPool();
        pathFinder = new PathFinder();
        tickManager = TickManager.Instance;
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            Vector3Int tileLocation = GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                tickManager.RegisterMovementClick(tileLocation);
            }
        }
    }

    public void SetTileClicked(Vector3Int tileLocation)
    {
        tileClicked = tileLocation;
    }
    public void OnGameTick()
    {
        //find path every tick for now, could make this better
        if (tileClicked != player.position)
        {
            List<Vector3Int> path = FindPath(GetTileLocation(player.position), tileClicked);
            DrawPath(path);

            //walk to the next for now
            player.position = path[1];
        }
    }

    private Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }

    private List<Vector3Int> FindPath(Vector3Int player, Vector3Int target)
    {
        ClearCurrentPathTiles();

        //TODO maybe handle path == null
        return pathFinder.FindPath(player, target);
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
            tmp = Instantiate(tileMarker);
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
