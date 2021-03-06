using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO check optimization at some point, using hash sets
//also creating the grid every time, have a look when it becomes a problem
public class PathFinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private PathNode[,] grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    private static readonly int GRID_RADIUS = 100;
    private static readonly int GRID_SIZE = (GRID_RADIUS * 2) + 1;

    public Vector2Int FindClosestWalkableTile(Vector2Int tile)
    {
        grid = CreateGrid();

        PathNode node = grid[tile.x + GRID_RADIUS, tile.y + GRID_RADIUS];

        if (node.isWalkable)
        {
            return tile;
        }

        PathNode closestEndNode = FindClosestWalkableNode(node);
        if (closestEndNode != null)
        {
            return new Vector2Int(closestEndNode.x - GRID_RADIUS, closestEndNode.y - GRID_RADIUS);
        }
        else
        {
            //TODO
            return new Vector2Int(0, 0);
        }
    }

    public List<Vector2Int> FindPath(Vector2Int player, Vector2Int target)
    {
        grid = CreateGrid();

        int startX = player.x + GRID_RADIUS;
        int startY = player.y + GRID_RADIUS;
        int targetX = target.x + GRID_RADIUS;
        int targetY = target.y + GRID_RADIUS;

        PathNode startNode = grid[startX, startY];
        PathNode endNode = grid[targetX, targetY];

        if (!endNode.isWalkable)
        {
            PathNode closestEndNode = FindClosestWalkableNode(endNode);
            if (closestEndNode != null)
            {
                endNode = closestEndNode;
            }
            else
            {
                //TODO
                return null;
            }
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                return ConvertPathToWorld(CalculatePath(endNode), player);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable)
                {
                    closedList.Add(neighbour);
                }
                else if (!closedList.Contains(neighbour))
                {
                    int newGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if (newGCost < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        neighbour.gCost = newGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.cameFromNode = currentNode;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        for (int x = -1; x <= 1 ; x++)
        {
            for (int y = -1; y <=1 ; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int neighbourX = node.x + x;
                int neighbourY = node.y + y;

                if (neighbourX >= 0 && neighbourX < GRID_SIZE && neighbourY >=0 && neighbourY < GRID_SIZE)
                {
                    neighbours.Add(grid[neighbourX, neighbourY]);
                }
            }
        }

        return neighbours;
    }

    private List<PathNode> CalculatePath(PathNode node)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(node);

        PathNode currentNode = node;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();

        return path;
    }


    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 1; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    private PathNode[,] CreateGrid()
    {
        PathNode[,] grid = new PathNode[GRID_SIZE, GRID_SIZE];
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                PathNode node = new PathNode(x, y);
                Vector3 worldPoint = new Vector3(x - GRID_RADIUS, 0, y - GRID_RADIUS);
                node.isWalkable = !Physics.CheckSphere(worldPoint, 1.0f, 1 << LayerMask.NameToLayer("Unwalkable"));
                grid[x, y] = node;
            }
        }

        return grid;
    }

    private List<Vector2Int> ConvertPathToWorld(List<PathNode> path, Vector2Int player)
    {
        List<Vector2Int> worldPath = new List<Vector2Int>();
        for (int i = 0; i < path.Count; i++)
        {
            int x = path[i].x - GRID_RADIUS;
            int y = path[i].y - GRID_RADIUS;
            worldPath.Add(new Vector2Int(x, y));
        }
        return worldPath;
    }

    private PathNode FindClosestWalkableNode(PathNode node)
    {
        for (int radius = 1; radius < GRID_RADIUS; radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) != radius)
                    {
                        if (Mathf.Abs(y) != radius)
                        {
                            continue;
                        }
                    }
                    int neighbourX = node.x + x;
                    int neighbourY = node.y + y;
                    PathNode closestNode = grid[neighbourX, neighbourY];

                    if (closestNode.isWalkable)
                    {
                        return closestNode;
                    }
                }
            }
        }

        return null;
    }
}
