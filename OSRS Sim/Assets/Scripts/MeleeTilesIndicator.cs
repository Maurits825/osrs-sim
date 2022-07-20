using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTilesIndicator : MonoBehaviour
{
    [SerializeField] private GameObject tileMarkerModel;
    private Npc npc;

    private List<GameObject> tilesObjects = new();


    private void Awake()
    {
        npc = GetComponent<Npc>();
    }

    private void Start()
    {
        List<Vector2Int> tiles = npc.GetMeleeTiles();

        foreach (Vector2Int tile in tiles)
        {
            tilesObjects.Add(Instantiate(tileMarkerModel, new Vector3(tile.x, 0, tile.y), Quaternion.identity));
        }
    }

    private void LateUpdate()
    {
        List<Vector2Int> tiles = npc.GetMeleeTiles();

        for (int i = 0; i < tiles.Count; i++)
        {
            Vector2Int tile = tiles[i];
            tilesObjects[i].transform.position = new Vector3(tile.x, 0, tile.y);
            tilesObjects[i].transform.rotation = Quaternion.identity;
        }
    }
}
