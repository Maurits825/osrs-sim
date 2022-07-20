using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO does it need to be abstract? for simple npc like man, cow prob not right?
public abstract class Npc : MonoBehaviour
{
    //TODO kind of scuffed with having to instatiate... is it worth?
    public NpcStates defaultNpcStates;
    [HideInInspector] public NpcStates npcStates;
    public NpcInfo defaultNpcInfo;
    public NpcInfo npcInfo;

    public Vector2Int spawnTile;
    public Vector2Int currentTile;

    protected IMovement movement;
    protected ICombat combat;

    private void Awake()
    {
        npcInfo = Instantiate(defaultNpcInfo);
        npcStates = Instantiate(defaultNpcStates);

        spawnTile = Utils.GetTileLocation(transform.position);
        currentTile = spawnTile;

        movement = GetComponent<IMovement>();
        combat = GetComponent<ICombat>();
    }

    protected virtual void Start()
    {
        Debug.Log("Register: " + npcInfo.npcName.ToString());
        NpcController.Instance.RegisterNpc(this);        
    }

    public void OnGameTick()
    {
        //regen stats, hp here?

        npcStates.currentState = npcStates.nextState;

        //these can affect the nextState
        movement.OnGameTick();
        npcStates.currentState = npcStates.nextState; // update states from movement
        combat.OnGameTick(); //can override the state set by movement, intended...

        //then update the current state?
        

        //TODO we could just not have this switch here and let movement and combat to there things
        //would allow move att same tick i think also?
        //onyl problem is that there could be an "invalid" move and att in same tick?
        //also makes the Move() fn in interface kinda useless...?
    }

    public bool IsInRange(Vector2Int tile, int attackRange)
    {
        if (attackRange == 0)
        {
            return isInMeleeRange(tile);
        }
        else
        {
            //we have to calc from the closes adj tile
            //easy way: just calc all distances and return if any is below
            List<Vector2Int> tiles = GetAllAdjacentTiles();
            foreach (Vector2Int adjTile in tiles)
            {
                int distance = Utils.GetChebyshevDistance(adjTile, tile);
                if (distance <= attackRange)
                {
                    return true;
                }
            }

            //TODO handle walking under?
            return false;
        }
    }

    public Vector2Int GetClosestAdjacentTile(Vector2Int tile)
    {
        List<Vector2Int> tiles = GetAllAdjacentTiles();

        int minDistance = int.MaxValue;
        Vector2Int closestTile = tiles[0];

        foreach (Vector2Int adjTile in tiles)
        {
            int distance = Utils.GetChebyshevDistance(adjTile, tile);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTile = adjTile;
            }
        }

        return closestTile;
    }

    public List<Vector2Int> GetMeleeTiles()
    {
        List<Vector2Int> tiles = new();

        for (int x = 0; x < npcInfo.size.x; x++)
        {
            int y = -1;
            tiles.Add(new Vector2Int(currentTile.x + x, currentTile.y + y));
        }

        for (int x = 0; x < npcInfo.size.x; x++)
        {
            int y = npcInfo.size.y;
            tiles.Add(new Vector2Int(currentTile.x + x, currentTile.y + y));
        }

        for (int y = 0; y < npcInfo.size.y; y++)
        {
            int x = -1;
            tiles.Add(new Vector2Int(currentTile.x + x, currentTile.y + y));
        }

        for (int y = 0; y < npcInfo.size.y; y++)
        {
            int x = npcInfo.size.x;
            tiles.Add(new Vector2Int(currentTile.x + x, currentTile.y + y));
        }

        return tiles;
    }

    private List<Vector2Int> GetAllAdjacentTiles()
    {
        List<Vector2Int> tiles = GetMeleeTiles();

        tiles.Add(new Vector2Int(currentTile.x - 1, currentTile.y - 1));
        tiles.Add(new Vector2Int(currentTile.x - 1, currentTile.y + npcInfo.size.y));
        tiles.Add(new Vector2Int(currentTile.x + npcInfo.size.x, currentTile.y - 1));
        tiles.Add(new Vector2Int(currentTile.x + npcInfo.size.x, currentTile.y + npcInfo.size.y));

        return tiles;
    }

    private bool isInMeleeRange(Vector2Int tile)
    {
        List<Vector2Int> tiles = GetMeleeTiles();
        return tiles.Contains(tile);
    }
}
