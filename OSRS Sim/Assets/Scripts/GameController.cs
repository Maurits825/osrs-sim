using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject tileMarker;

    [SerializeField] private GameStates gameState;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Movement movement;
    [SerializeField] private CombatController combatController;

    private Vector3Int tileClicked;
    private Vector3Int enemyTileClicked;
    private Enemy enemyClicked;
    private Vector3Int lastTile;

    private void Start()
    {
    }

    private void Update() //TODO buffer the next state transition in a var?
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        //TODO refactor to maybe use one raycast? inverse layermask with unwalkable?
        if (Physics.Raycast(ray, out raycastHit, float.MaxValue, enemyMask))
        {
            //TODO get tile location from enemy
            Vector3Int tileLocation = GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                enemyTileClicked = tileLocation;
                enemyClicked = raycastHit.collider.GetComponent<Enemy>();
                gameState.currentState = GameStates.States.EnemyClicked;
            }
        }
        else if (Physics.Raycast(ray, out raycastHit, float.MaxValue, groundMask))
        {
            Vector3Int tileLocation = GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                tileClicked = tileLocation;
                gameState.currentState = GameStates.States.TileClicked;
            }
        }
    }

    public void OnGameTick()
    {
        GameStates.States nextGameState = gameState.currentState;
        movement.OnGameTick();

        switch (gameState.currentState)
        {
            case GameStates.States.Idle:
                break;

            case GameStates.States.TileClicked:
                nextGameState = HandleGenericTileClick(tileClicked, GameStates.States.Moving, GameStates.States.Idle);
                break;

            case GameStates.States.Moving:
                if (HandleGenericMoving(tileClicked))
                {
                    nextGameState = GameStates.States.Idle;
                }
                break;

            case GameStates.States.EnemyClicked:
                combatController.currentEnemy = enemyClicked;
                nextGameState = HandleGenericTileClick(enemyTileClicked, GameStates.States.MovingToEnemy, GameStates.States.Attacking);
                break;

            case GameStates.States.MovingToEnemy:
                if (HandleGenericMoving(enemyTileClicked))
                {
                    nextGameState = GameStates.States.Attacking;
                }
                break;

            case GameStates.States.Attacking:
                combatController.OnGameTick();
                break;

            default:
                break;
        }

        lastTile = movement.CurrentPlayerTile;
        gameState.currentState = nextGameState;
        
    }

    private Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }

    private GameStates.States HandleGenericTileClick(Vector3Int tile, GameStates.States notAtTile, GameStates.States atTile)
    {
        if (tile != movement.CurrentPlayerTile)
        {
            Vector3Int nextTile = movement.ProcessMovement(tile);
            playerController.OnGameTick(nextTile);
            return notAtTile;
        }
        else
        {
            return atTile;
        }
    }

    private bool HandleGenericMoving(Vector3Int tile)
    {
        Vector3Int nextTile = movement.ProcessMovement(tile);
        if (lastTile == movement.CurrentPlayerTile)
        {
            return true;
        }
        else
        {

            playerController.OnGameTick(nextTile);
            return false;
        }
    }
}
