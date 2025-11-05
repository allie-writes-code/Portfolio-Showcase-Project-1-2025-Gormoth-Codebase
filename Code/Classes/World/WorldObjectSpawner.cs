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
        SpawnInitialBuildings();
        SpawnPlayer();
        levelLoadedBroadcaster.InvokeMe();
    }

    private void SpawnInitialBuildings()
    {
        foreach (WorldObjecfSpawnChance chance in spawnData.initialSpawnBuildings)
        {
            for (int i = 0; i < chance.HowManyToSpawn; i++)
            {
                Vector3 spawnPos = GetClearPosWithDistance(chance.MinDistance(grid.GridMaxX), chance.MaxDistance(grid.GridMaxX));
                GameObject newBuilding = Instantiate(chance.worldObjectPrefab, spawnPos, chance.worldObjectPrefab.transform.rotation);
                grid.OccupyNode(spawnPos);
                grid.SetNodeObject(spawnPos, newBuilding);
            }
        }
    }

    private void SpawnInitialResourceDroppers()
    {
        int totalToSpawn = spawnData.resourceDroppersMaxAmt;
        spawnData.currentResourceDroppersTotal = 0;
        List<WorldObjecfSpawnChance> selectedChances = new List<WorldObjecfSpawnChance>();

        for (int i = 0; i <= totalToSpawn;)
        {
            selectedChances.Clear();

            // Get each applicable object based on its spawn chance data.
            foreach (WorldObjecfSpawnChance chance in spawnData.resourceDroppers)
            {
                if (chance.SpawnRollPass())
                {
                    selectedChances.Add(chance);
                }
            }

            if (selectedChances.Count > 0)
            {
                WorldObjecfSpawnChance selected = selectedChances[Random.Range(0, selectedChances.Count)];

                // We use GridMaxX for both because we want the values as percentages of the max.
                Vector3 spawnPos = GetClearPosWithDistance(selected.MinDistance(grid.GridMaxX), selected.MaxDistance(grid.GridMaxX));

                GameObject newDropperObject = Instantiate(selected.worldObjectPrefab, spawnPos, selected.worldObjectPrefab.transform.rotation);
                newDropperObject.name = "Resource Dropper";
                grid.OccupyNode(spawnPos);
                grid.SetNodeObject(spawnPos, newDropperObject);
                i++;
            }
        }
    }

    private Vector3 GetClearPosWithDistance(int min, int max)
    {
        bool posFound = false;
        Vector3 ranPos = Vector3.zero;
        int breakcount = 0;
        while (posFound == false)
        {
            breakcount++;
            if (breakcount > 1000) 
            {
                Debug.Log("Break point in GetClearPosWithDistance hit.");
                return Vector3.zero; 
            }
            ranPos = new Vector3(Random.Range(-max, max + 1), 1, Random.Range(-max, max + 1));

            // Check min distances, avoids having to use Vector3.Distance
            if (ranPos.x <= -min || ranPos.x >= min
                || ranPos.z <= -min || ranPos.z >= min)
            {
                if (grid.IsNodeClear(ranPos)) posFound = true;
            }
        }

        return ranPos;
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
