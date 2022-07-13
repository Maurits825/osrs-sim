using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject tileMarker;
    [SerializeField] private GameObject playerObject;
    private enum States
    {
        Idle = 0,
        TileClicked,
        Moving,
    }

    private PlayerController playerController;
    private Movement movement;

    private States gameState = States.Idle;

    private Vector3Int tileClicked;

    private void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        movement = playerObject.GetComponent<Movement>();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundMask))
        {
            Vector3Int tileLocation = GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                tileClicked = tileLocation;
                gameState = States.TileClicked;
            }
        }
    }

    public void OnGameTick()
    {
        movement.OnGameTick();

        switch (gameState)
        {
            case States.Idle:
                break;
            case States.TileClicked:
                if (tileClicked != movement.CurrentPlayerTile)
                {
                    Vector3Int nextTile = movement.ProcessMovement(tileClicked);
                    playerController.OnGameTick(nextTile);
                }
                break;
            case States.Moving:
                break;
            default:
                break;
        }
        
    }

    private Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Handles.Label(movement.CurrentPlayerTile + (2 * Vector3.up), gameState.ToString());
        }
    }
}
