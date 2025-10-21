using System.Collections.Generic;
using UnityEngine;

//! ScriptableObject class, handles tracking and spawning world objects in game.
[CreateAssetMenu(fileName = "New World Object Spawner", menuName = "Scriptable Objects/World/World Object Spawner", order = 5)]
public class WorldObjectSpawner : ScriptableObject
{
    [SerializeField]
    private DelegateListener levelSpawnedListener;

    [SerializeField]
    private DelegateListener resourceDropperDiedListener;

    [SerializeField]
    private WorldGridManager grid;

    [SerializeField]
    private WorldSpawnData spawnData;

    [SerializeField]
    private DelegateBroadcaster levelLoadedBroadcaster;

    private void OnEnable()
    {
        levelSpawnedListener.RegisterFunction(LevelLoaded);
        resourceDropperDiedListener.RegisterFunction(ResourceDropperDied);
    }

    private void OnDisable()
    {
        levelSpawnedListener.DeregisterFunction(LevelLoaded);
        resourceDropperDiedListener.DeregisterFunction(ResourceDropperDied);
    }

    private void LevelLoaded()
    {
        SpawnInitialResourceDroppers();
        SpawnPlayer();
        levelLoadedBroadcaster.InvokeMe();
    }

    private void SpawnInitialResourceDroppers()
    {
        int totalToSpawn = spawnData.resourceDroppersMaxAmt;
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

    private void SpawnPlayer()
    {
        if (spawnData.playerPrefab != null)
        {
            GameObject playerObject = Instantiate(spawnData.playerPrefab, new Vector3(0, 1, 0), spawnData.playerPrefab.transform.rotation);
            playerObject.name = "Player";
        }
    }

    private void ResourceDropperDied()
    {
        spawnData.currentResourceDroppersTotal--;
    }
}
