using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerVariables playerVariables;

    [SerializeField] private Transform player;

    private const float modelRotationSpeed = 250;
    private float modelMoveSpeed;
    private const float modelMoveSpeedRun = 3.0f; //TODO make this faster?
    private const float modelMoveSpeedWalk = 1.5f;

    private Queue<Vector3Int> nextTiles= new();
    private float modelDistanceThreshold = 0.01f;

    private void Start()
    {
    }

    private void Update()
    {
        SetPositionAndRotation();
    }

    public void OnGameTick(Vector3Int nextTile)
    {
        nextTiles.Enqueue(nextTile);
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
