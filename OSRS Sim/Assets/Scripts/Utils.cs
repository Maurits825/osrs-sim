using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3Int GetTileLocation(Vector3 worldLocation)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldLocation.x),
            Mathf.RoundToInt(0),
            Mathf.RoundToInt(worldLocation.z));
    }
}
