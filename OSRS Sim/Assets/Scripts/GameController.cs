using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject tileMarker;

    [SerializeField] private GameStates gameState;
    [SerializeField] private PlayerVariables playerVariables;

    //[SerializeField] private CombatController combatController;

    [SerializeField] private Npc player;
    [SerializeField] private InputController inputController;

    private Vector2Int tileClicked;
    private Vector2Int enemyTileClicked;
    private Vector2Int lastTile;

    private Npc npc;

    private void Start()
    {
    }

    private void Update() //TODO buffer the next state transition in a var?
    {
    }

    public void OnGameTick()
    {
        GameStates.States nextGameState = gameState.currentState;
        gameState.currentState = GameStates.States.Idle; //TODO for now

        //movement.OnGameTick();
        inputController.OnGameTick();
        NpcController.Instance.OnGameTick();
        player.OnGameTick();

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
                //combatController.currentEnemy = enemyClicked;
                nextGameState = HandleGenericTileClick(enemyTileClicked, GameStates.States.MovingToEnemy, GameStates.States.Attacking);
                break;

            case GameStates.States.MovingToEnemy:
                if (HandleGenericMoving(enemyTileClicked))
                {
                    nextGameState = GameStates.States.Attacking;
                }
                break;

            case GameStates.States.Attacking:
                //combatController.OnGameTick();
                break;

            default:
                break;
        }

        //lastTile = playerVariables.currentTile;
        gameState.currentState = nextGameState;
        
    }

    private GameStates.States HandleGenericTileClick(Vector2Int tile, GameStates.States notAtTile, GameStates.States atTile)
    {
        if (tile != npc.currentTile)
        {
            //Vector3Int nextTile = movement.ProcessMovement(tile);
            //playerController.OnGameTick(nextTile);
            return notAtTile;
        }
        else
        {
            return atTile;
        }
    }

    private bool HandleGenericMoving(Vector2Int tile)
    {
        //Vector3Int nextTile = movement.ProcessMovement(tile);
        if (lastTile == npc.currentTile)
        {
            return true;
        }
        else
        {

            //playerController.OnGameTick(nextTile);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        playerVariables.ResetValues();
        gameState.ResetValues();
    }
}
