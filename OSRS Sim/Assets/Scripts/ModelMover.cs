using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMover : MonoBehaviour
{
    [SerializeField] private Transform model;

    private Npc npc;
    private ICombat combat;

    private Queue<Vector2Int> nextTiles = new();
    private Queue<Vector2> nextDirections = new();
    private Queue<int> tilesMoved = new();
    private Vector2Int previousTile;

    private const float modelRotationSpeed = 250;
    private float modelMoveSpeed;
    private const float modelMoveSpeedWalk = 1.7f;

    private float modelDistanceThreshold = 0.01f;
    private float modelAngleThreshold = 0.01f;

    private void OnGameTick()
    {
        if (npc.currentTile != previousTile)
        {
            nextTiles.Enqueue(npc.currentTile);
            nextDirections.Enqueue(npc.currentTile - previousTile);

            int x = Mathf.Abs(npc.currentTile.x - previousTile.x);
            int y = Mathf.Abs(npc.currentTile.y - previousTile.y);
            int tiles = Mathf.Max(x, y);

            tilesMoved.Enqueue(tiles);

            previousTile = npc.currentTile;
        }

        //TODO currently empty and requeue npc every tick, could be better
        switch (npc.npcStates.currentState)
        {
            case NpcStates.States.MovingToNpc:
            case NpcStates.States.AttackingNpc:
                nextDirections.Clear();
                break;
        }
    }

    private void Awake()
    {
        npc = GetComponent<Npc>();
        combat = GetComponent<ICombat>();
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
            Vector2Int currentTile = nextTiles.Peek();
            int tiles = tilesMoved.Peek();
            if (Vector2.Distance(new Vector2(model.position.x, model.position.z), currentTile) <= modelDistanceThreshold)
            {
                nextTiles.Dequeue();
                tilesMoved.Dequeue();
                if (nextTiles.Count > 0)
                {
                    currentTile = nextTiles.Peek();
                    tiles = tilesMoved.Peek();
                }
            }

            modelMoveSpeed = modelMoveSpeedWalk * tiles;
            Vector3 position = Vector3.MoveTowards(model.position, new Vector3(currentTile.x, model.position.y, currentTile.y), modelMoveSpeed * Time.deltaTime);
            model.position = position;
        }

        if (nextDirections.Count > 0)
        {
            Vector2 currentDirection = nextDirections.Peek();
            if (Vector3.Angle(model.forward, new Vector3(currentDirection.x, 0, currentDirection.y)) <= modelAngleThreshold)
            {
                nextDirections.Dequeue();
                if (nextDirections.Count > 0)
                {
                    currentDirection = nextDirections.Peek();
                }
            }

            SetRotation(currentDirection);
        }

        switch (npc.npcStates.currentState)
        {
            case NpcStates.States.MovingToNpc:
            case NpcStates.States.AttackingNpc:
                SetRotation(GetCenterTile(combat.GetNpcTarget()) - new Vector2(model.position.x, model.position.z));
                break;
        }
    }

    private void SetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        model.transform.rotation = Quaternion.RotateTowards(model.rotation, q, Time.deltaTime * modelRotationSpeed);
    }

    private Vector2 GetCenterTile(Npc npc)
    {
        float x = npc.currentTile.x + ((npc.npcInfo.size.x - 1) / 2f);
        float y = npc.currentTile.y + ((npc.npcInfo.size.y - 1) / 2f);

        return new Vector2(x, y);
    }
}
