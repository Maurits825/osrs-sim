using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerVariables playerVariables;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject tileMarker;
    [SerializeField] private Transform player;

    private const float modelRotationSpeed = 250;
    private float modelMoveSpeed;
    private const float modelMoveSpeedRun = 3.5f; //TODO make this faster?
    private const float modelMoveSpeedWalk = 1;

    private TickManager tickManager;
    private Movement movement;
    private Vector3Int tileClicked;

    //private Vector3Int nextTile;
    private Queue<Vector3Int> nextTiles= new();
    //private Vector3 direction;
    private float modelDistanceThreshold = 0.01f;

    private void Start()
    {
        tickManager = TickManager.Instance;
        movement = GetComponent<Movement>();
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

        SetPositionAndRotation();
    }

    public void SetTileClicked(Vector3Int tileLocation)
    {
        tileClicked = tileLocation;
    }
    public void OnGameTick()
    {
        //find path every tick for now, could make this better
        Vector3Int? nextTile = movement.ProcessMovement(tileClicked);
        if (nextTile.HasValue)
        {
            nextTiles.Enqueue((Vector3Int)nextTile);
        }
    }

    private Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }

    private void SetPositionAndRotation()
    {
        if (nextTiles.Count > 0)
        {
            Vector3Int currentTile = nextTiles.Peek();
            Vector3 direction = currentTile - player.position;
            if (Vector3.Distance(player.position, currentTile) <= modelDistanceThreshold)
            {
                nextTiles.Dequeue();

                if (nextTiles.Count > 0)
                {
                    currentTile = nextTiles.Peek();
                    direction = currentTile - player.position;
                }
            }

            //TODO if moving only one tile while running should be walking...
            modelMoveSpeed = playerVariables.isRunning ? modelMoveSpeedRun : modelMoveSpeedWalk;

            //TODO rotation doesnt always finish when moving one tile
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 position = Vector3.MoveTowards(player.position, currentTile, modelMoveSpeed * Time.deltaTime);
            player.SetPositionAndRotation(position, Quaternion.RotateTowards(player.rotation, q, Time.deltaTime * modelRotationSpeed));
        }
    }
}
