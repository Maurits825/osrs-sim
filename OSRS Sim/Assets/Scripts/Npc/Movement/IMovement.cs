using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement : IGameTick
{
    public void Move(); // todo
    public void SetTargetTile(Vector3Int target);
}
