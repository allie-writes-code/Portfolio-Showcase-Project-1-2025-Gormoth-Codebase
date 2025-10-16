using UnityEngine;
using System.Collections.Generic;

//! ScriptableObject class for defining world object managers. This class works with the WorldGridManager and handles the spawning of objects into a world / level.
[CreateAssetMenu(fileName = "New World Object Manager", menuName = "Scriptable Objects/World/World Object Manager", order = 2)]
public class WorldObjectManager : ScriptableObject
{
    [SerializeField]
    private WorldGridManager grid;

    [SerializeField]
    private WorldSpawnData spawnData;

    //! Public method to generate a new world.
    public void GenerateNewWorld()
    {
        grid.GenerateNewWorldGrid(spawnData.worldSizeX, spawnData.worldSizeY);

        if (spawnData.spawnBoundary && spawnData.boundaryPrefab != null)
        {
            GenerateWorldBoundary();
        }

        if (spawnData.spawnSpaghettiWalls && spawnData.spaghettiWallPrefab != null)
        {
            GenerateSpaghettiWalls();
        }

        if (spawnData.corePrefab != null)
        {
            SpawnCore();
        }

        PopulateResourceDroppers();
    }

    public void GenerateSpaghettiWalls()
    {
        List<Vector3> initialSpawnPositions = new List<Vector3>();
        List<Vector3> finalSpawnPosition = new List<Vector3>();

        for (int i = 0; i <= spawnData.spagWallsToSpawnSeparate;)
        {
            bool posFound = false;

            while (posFound == false)
            {
                int x = Random.Range(grid.GridMinX, grid.GridMaxX + 1);
                int y = Random.Range(grid.GridMinY, grid.GridMaxY + 1);
                Vector3 wallPos = new Vector3(x, 1, y);
                if (grid.HasNode(wallPos) && grid.IsNodeClear(wallPos))
                {
                    float distance = Vector3.Distance(wallPos, Vector3.zero);

                    if (distance >= spawnData.spagWallDistanceFromCentre)
                    {
                        initialSpawnPositions.Add(wallPos);
                        finalSpawnPosition.Add(wallPos);
                        grid.OccupyNode(wallPos);
                        posFound = true;
                    }
                }
            }

            i++;
        }

        List<Vector3> doNotReuse = new List<Vector3>();

        for (int i = spawnData.spagWallsToSpawnSeparate; i <= spawnData.spagWallsTotalVolume;)
        {
            bool posFound = false;

            while (posFound == false)
            {
                Vector3 existingWallPos = initialSpawnPositions[Random.Range(0, initialSpawnPositions.Count)];
                Vector3 newSpawnPos = existingWallPos + neighbourPositions[Random.Range(0, neighbourPositions.Length)];
                if (doNotReuse.Contains(newSpawnPos) == false && grid.HasNode(newSpawnPos) && grid.IsNodeClear(newSpawnPos))
                {
                    finalSpawnPosition.Add(newSpawnPos);
                    initialSpawnPositions.Add(newSpawnPos);
                    grid.OccupyNode(newSpawnPos);
                    posFound = true;

                    doNotReuse.Add(existingWallPos);
                    initialSpawnPositions.Remove(existingWallPos);
                }
            }

            i++;
        }

        if (finalSpawnPosition.Count > 0)
        {
            foreach(Vector3 pos in finalSpawnPosition)
            {
                GameObject newSpagWallObject = Instantiate(spawnData.spaghettiWallPrefab, pos, spawnData.spaghettiWallPrefab.transform.rotation);
                newSpagWallObject.name = "Spaghetti Wall";
                grid.OccupyNode(pos);
            }
        }
    }

    public void PopulateResourceDroppers()
    {
        int totalToSpawn = Random.Range(spawnData.resourceDroppersMinAmt, spawnData.resourceDroppersMaxAmt + 1);
        List<GameObject> selectedPrefabs = new List<GameObject>();
        Vector3 ranPos = Vector3.zero;
        for (int i = 0; i <= totalToSpawn;)
        {
            selectedPrefabs.Clear();
            bool posFound = false;

            int breaker = 0;

            // Loop until we find a position in grid that's unoccupied.
            while (posFound == false)
            {
                ranPos = new Vector3(Random.Range(grid.GridMinX, grid.GridMaxX + 1), 1, Random.Range(grid.GridMinY, grid.GridMaxY + 1));

                if (grid.IsNodeClear(ranPos)) posFound = true;

                if (breaker > 1000)
                {
                    Debug.Log("Breakpoint hit looking for pos.");
                    posFound = true;
                }

                breaker++;
            }

            float distance = Vector3.Distance(Vector3.zero, ranPos);

            // Get each applicable object based on its spawn chance data.
            foreach (WorldObjecfSpawnChance chance in spawnData.resourceDroppers)
            {
                if (distance >= chance.MinDistance(grid.GridMaxX))
                {
                    if (chance.SpawnRollPass())
                    {
                        selectedPrefabs.Add(chance.worldObjectPrefab);
                    }
                }
            }

            // Instantiate one of the objects from the list.
            if (selectedPrefabs.Count > 0)
            {
                GameObject prefab = selectedPrefabs[Random.Range(0, selectedPrefabs.Count)];
                GameObject newDropperObject = Instantiate(prefab, ranPos, prefab.transform.rotation);
                newDropperObject.name = "Resource Dropper";

                grid.OccupyNode(ranPos);
                grid.SetNodeObject(ranPos, newDropperObject);

                i++;
            }
        }
    }

    public void SpawnCore()
    {
        Vector3 spawnPos = new Vector3(0, 1, 0);
        GameObject core = Instantiate(spawnData.corePrefab, spawnPos, spawnData.corePrefab.transform.rotation);
        core.name = "Core";
        grid.OccupyNode(spawnPos);
        grid.SetNodeObject(spawnPos, core);
    }

    public void GenerateWorldBoundary()
    {
        for (int x = grid.GridMinX; x <= grid.GridMaxX; x++)
        {
            for (int y = grid.GridMinY; y <= grid.GridMaxY; y++)
            {
                if (x == grid.GridMinX || x == grid.GridMaxX
                    || y == grid.GridMinY || y == grid.GridMaxY)
                {
                    GameObject newBoundaryObject = Instantiate(spawnData.boundaryPrefab, new Vector3(x, 1, y), spawnData.boundaryPrefab.transform.rotation);
                    newBoundaryObject.name = "World Boundary Object " + x + ":" + y;

                    Vector3 nodePos = new Vector3(x, 1, y);
                    grid.OccupyNode(nodePos);
                    grid.SetNodeObject(nodePos, newBoundaryObject);
                }
            }
        }
    }

    private Vector3[] neighbourPositions =
    {
        new Vector3(-1,0,0),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(0,0,1),
    };
}
