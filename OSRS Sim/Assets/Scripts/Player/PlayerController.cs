using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject tileMarker;
    [SerializeField] private Transform player;

    [SerializeField] private float modelRotationSpeed;
    [SerializeField] private float modelMoveSpeed;

    private TickManager tickManager;
    private Movement movement;
    private Vector3Int tileClicked;

    Vector3Int nextTile;
    Vector3 direction;

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
        nextTile = movement.ProcessMovement(tileClicked);
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
        if (nextTile != player.position)
        {
            direction = nextTile - player.position;
        }

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 position = Vector3.MoveTowards(player.position, nextTile, modelMoveSpeed * Time.deltaTime);
        player.SetPositionAndRotation(position, Quaternion.RotateTowards(player.rotation, q, Time.deltaTime * modelRotationSpeed));
    }
}
