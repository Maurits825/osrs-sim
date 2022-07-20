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

    public static int GetChebyshevDistance(Vector2Int startTile, Vector2Int targetTile)
    {
        int x = Mathf.Abs(targetTile.x - startTile.x);
        int y = Mathf.Abs(targetTile.y - startTile.y);
        return Mathf.Max(x, y);
    }
}
