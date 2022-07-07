using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x, y;

    public int gCost, hCost;

    public bool isWalkable;

    public PathNode cameFromNode;

    public PathNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}