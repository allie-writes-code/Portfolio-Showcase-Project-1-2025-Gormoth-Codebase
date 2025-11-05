using UnityEngine;
using System.Collections.Generic;

//! ScriptableObject class for spawning the initial level (boundaries, walls, etc).
[CreateAssetMenu(fileName = "New Level Spawner", menuName = "Scriptable Objects/World/Level Spawner", order = 2)]
public class LevelSpawner : ScriptableObject
{
    [SerializeField]
    private WorldGridManager grid;

    [SerializeField]
    private WorldSpawnData spawnData;

    [SerializeField]
    private DelegateBroadcaster levelSpawnedBroadcast;

    //! Public method to generate a new world.
    public void GenerateNewWorld()
    {
        grid.GenerateNewWorldGrid(spawnData.worldSizeX, spawnData.worldSizeY);

        spawnData.boundaryPositions = GetBoundaries();

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

        levelSpawnedBroadcast.InvokeMe();
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
                int x = Random.Range(grid.GridMinX + 25, grid.GridMaxX - 24);
                int y = Random.Range(grid.GridMinY + 25, grid.GridMaxY - 24);
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
        foreach(Vector3 p in spawnData.boundaryPositions)
        {
            GameObject newBoundaryObject = Instantiate(spawnData.boundaryPrefab, new Vector3(p.x, 1, p.z), spawnData.boundaryPrefab.transform.rotation);
            newBoundaryObject.name = "World Boundary Object " + p.x + ":" + p.z;

            Vector3 nodePos = new Vector3(p.x, 1, p.z);
            grid.OccupyNode(nodePos);
            grid.SetNodeObject(nodePos, newBoundaryObject);
        }
    }

    private List<Vector3> GetBoundaries()
    {
        List<Vector3> boundaryLocs = new List<Vector3>();
        
        for (int x = grid.GridMinX; x <= grid.GridMaxX; x++)
        {
            for (int y = grid.GridMinY; y <= grid.GridMaxY; y++)
            {
                if (x == grid.GridMinX || x == grid.GridMaxX
                    || y == grid.GridMinY || y == grid.GridMaxY)
                {
                    boundaryLocs.Add(new Vector3(x, 1, y));
                }
            }
        }

        return boundaryLocs;
    }

    private Vector3[] neighbourPositions =
    {
        new Vector3(-1,0,0),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(0,0,1),
    };
}
