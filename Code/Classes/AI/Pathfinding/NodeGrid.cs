using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node Grid", menuName = "Scriptable Objects/AStar/Node Grid", order = 1)]
public class NodeGrid : ScriptableObject
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public PathNode[,] grid;

    [HideInInspector]
    public float nodeDiameter;

    int gridSizeX, gridSizeY;

    public void Init()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new PathNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = Vector3.zero - 
            Vector3.right * gridWorldSize.x / 2 - 
            Vector3.forward * gridWorldSize.y /2;
        Debug.Log("Path grid created - Bottom left is " + worldBottomLeft);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * nodeDiameter)
                    + Vector3.forward * (y * nodeDiameter);
                worldPoint.y = 1;

                bool walkable = IsNodeWalkable(worldPoint);
                grid[x, y] = new PathNode(walkable, worldPoint, x, y);
            }
        }
    }

    public void UpdateWalkableAtPoint(Vector3 pos)
    {
        PathNode centreNode = NodeFromWorldPoint(pos);
        centreNode.walkable = IsNodeWalkable(centreNode.worldPosition);
        List<PathNode> neighbours = GetNeighbours(centreNode);

        foreach(PathNode n in neighbours)
        {
            n.walkable = IsNodeWalkable(n.worldPosition);
        }
    }

    private bool IsNodeWalkable(Vector3 pos)
    {
        return !Physics.CheckSphere(pos, (nodeRadius / 2), unwalkableMask);
    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        for (int x = - 1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX
                    && checkY >=0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
}