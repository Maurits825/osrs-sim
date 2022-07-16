using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMover : MonoBehaviour
{
    [SerializeField] private Transform model;
    private Npc npc;
    private Queue<Vector3Int> nextTiles = new();
    private Queue<Vector3> nextDirections = new();
    private Queue<int> tilesMoved = new();
    private Vector3Int previousTile;

    private const float modelRotationSpeed = 250;
    private float modelMoveSpeed;
    private const float modelMoveSpeedRun = 3.0f; //TODO make this faster? also just make it twice as fast as walk
    private const float modelMoveSpeedWalk = 1.5f; //or to speed = tiles moved * x

    private float modelDistanceThreshold = 0.01f;
    private float modelAngleThreshold = 0.01f;

    private void OnGameTick()
    {
        if (npc.currentTile != previousTile)
        {
            nextTiles.Enqueue(npc.currentTile);
            nextDirections.Enqueue(npc.currentTile - previousTile);

            int x = Mathf.Abs(npc.currentTile.x - previousTile.x);
            int y = Mathf.Abs(npc.currentTile.z - previousTile.z);
            int tiles = Mathf.Max(x, y);

            tilesMoved.Enqueue(tiles);

            previousTile = npc.currentTile;//todo is this the probelm]
        }
    }

    private void Awake()
    {
        npc = GetComponent<Npc>();
    }

    private void Start()
    {
        previousTile = npc.spawnTile;
        EventController.Instance.OnGameTick += OnGameTick;
    }

    
    private void Update()
    {
        SetPositionAndRotation();
    }

    private void SetPositionAndRotation()
    {
        if (nextTiles.Count > 0)
        {
            Vector3Int currentTile = nextTiles.Peek();
            int tiles = tilesMoved.Peek();
            if (Vector2.Distance(new Vector2(model.position.x, model.position.z), new Vector2(currentTile.x, currentTile.z)) <= modelDistanceThreshold)
            {
                nextTiles.Dequeue();
                tilesMoved.Dequeue();
                if (nextTiles.Count > 0)
                {
                    currentTile = nextTiles.Peek();
                    tiles = tilesMoved.Peek();
                }
                else
                {
                    return;
                }
            }

            modelMoveSpeed = tiles >= 2 ? modelMoveSpeedRun : modelMoveSpeedWalk;
            Vector3 position = Vector3.MoveTowards(model.position, new Vector3(currentTile.x, model.position.y, currentTile.z), modelMoveSpeed * Time.deltaTime);
            model.position = position;
        }

        if (nextDirections.Count > 0)
        {
            Vector3 currentDirection = nextDirections.Peek();
            if (Vector3.Angle(model.eulerAngles, currentDirection) <= modelAngleThreshold)
            {
                nextDirections.Dequeue();
                if (nextDirections.Count > 0)
                {
                    currentDirection = nextDirections.Peek();
                }
                else
                {
                    return;
                }
            }

            float angle = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
            Quaternion.RotateTowards(model.rotation, q, Time.deltaTime * modelRotationSpeed);
        }
    }
}
