using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject tileMarker;
    [SerializeField] private Transform player;

    private TickManager tickManager;
    private Movement movement;
    private Vector3Int tileClicked;

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
            player.position = movement.ProcessMovement(GetTileLocation(player.position), tileClicked);
        }
    }

    private Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }
}
