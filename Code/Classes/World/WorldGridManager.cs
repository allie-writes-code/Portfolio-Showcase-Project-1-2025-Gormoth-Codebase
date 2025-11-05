using System;
using System.Collections.Generic;
using UnityEngine;

//! ScriptableObject class to define a world grid manager object.
[CreateAssetMenu(fileName = "New World Grid Manager", menuName = "Scriptable Objects/World/World Grid Manager", order = 1)]
public class WorldGridManager : ScriptableObject
{
    //! Dictionary to store world grid. Uses a Tuple as the key and a WorldGridNode as the value.
    private Dictionary<Tuple<int, int>, WorldGridNode> worldGrid = new Dictionary<Tuple<int, int>, WorldGridNode>();

    //! Private ints to store grid boundary values.
    private int gridMinX, gridMinY, gridMaxX, gridMaxY;

    public int GridMinX { get { return gridMinX; } }
    public int GridMinY { get { return gridMinY; } }
    public int GridMaxX { get { return gridMaxX; } }
    public int GridMaxY { get { return gridMaxY; } }

    [SerializeField]
    private NodeGrid pathGrid;

    //! Method to check for a WorldGridNode in the grid, and update its isOccupied bool to the provided value if found.
    public void OccupyNode(Vector3 pos, bool occupied = true)
    {
        if (HasNode(pos))
        {
            GetNodeFromPos(pos).isOccupied = occupied;
            pathGrid.UpdateWalkableAtPoint(pos);
        }
    }

    public void SetNodeObject(Vector3 pos, GameObject go)
    {
        if (HasNode(pos))
        {
            GetNodeFromPos(pos).gridObject = go;
        }
    }

    public bool IsNodeClear(Vector3 pos)
    {
        bool nodeClear = true;

        if (HasNode(pos))
        {
            if (GetNodeFromPos(pos).isOccupied == true) nodeClear = false;
        }

        return nodeClear;
    }

    //! return worldGrid[TupleFromVector3(pos)];Method to return node from Vector3.
    public WorldGridNode GetNodeFromPos(Vector3 pos)
    {
        if (HasNode(pos))
        {
            return worldGrid[TupleFromVector3(pos)];
        }
        
        return null;
    }

    //! Method to convert Vector3 into Tuple.
    public Tuple<int,int> TupleFromVector3(Vector3 pos)
    {
        return Tuple.Create(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
    }

    //! Method to return bool, checks if grid contains a node for given Vector3.
    public bool HasNode(Vector3 pos)
    {
        bool nodeExists = false;

        if (worldGrid.ContainsKey(TupleFromVector3(pos)))
        {
            nodeExists = true;
        }

        return nodeExists;
    }

    //! Method to generate a new world grid.
    public void GenerateNewWorldGrid(int sizeX, int sizeY)
    {
        worldGrid = new Dictionary<Tuple<int, int>, WorldGridNode>();

        int xHalf = sizeX / 2;
        int yHalf = sizeY / 2;

        gridMinX = -xHalf;
        gridMinY = -yHalf;
        gridMaxX = xHalf;
        gridMaxY = yHalf;

        for (int x = gridMinX; x <= gridMaxX; x++)
        {
            for (int y = gridMinY; y <= gridMaxY; y++)
            {
                WorldGridNode newNode = new WorldGridNode();
                newNode.worldPos = new Vector3(x, 1, y);
                worldGrid.Add(Tuple.Create(x, y), newNode);
            }
        }
    }

    //! Internal class to store data on nodes in grid.
    public class WorldGridNode
    {
        public bool isOccupied = false;
        public GameObject gridObject = null;
        public Vector3 worldPos = Vector3.zero;
    }
}
