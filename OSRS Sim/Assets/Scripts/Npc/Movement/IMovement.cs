using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement : IGameTick
{
    public void Move();
    public void SetTargetTile(Vector2Int target);
}
