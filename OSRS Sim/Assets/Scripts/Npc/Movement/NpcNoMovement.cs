using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcNoMovement : MonoBehaviour, IMovement
{
    public void Move()
    {
        return;
    }

    public void OnGameTick()
    {
        return;
    }

    public void SetTargetNpc(Npc target)
    {
        return;
    }

    public void SetTargetTile(Vector2Int target)
    {
        return;
    }

    public Vector2Int GetTargetTile()
    {
        return new Vector2Int(0, 0);
    }
}
