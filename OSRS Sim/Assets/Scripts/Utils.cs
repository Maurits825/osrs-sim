using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(worldLocation.z));
    }

    public static Vector3 GetWorldLocation(Vector2Int tile)
    {
        return new Vector3(tile.x, 0, tile.y);
    }
}
