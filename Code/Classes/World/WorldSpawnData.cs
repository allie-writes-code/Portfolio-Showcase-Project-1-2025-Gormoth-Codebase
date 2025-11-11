using System;
using System.Collections.Generic;
using UnityEngine;

//! ScriptableObject class for defining world spawn data instances. This class holds initial world generation variables as well as maintains the world in game.
[CreateAssetMenu(fileName = "New World Spawn Data", menuName = "Scriptable Objects/World/World Spawn Data", order = 3)]
public class WorldSpawnData : ScriptableObject
{
    public int worldSizeX = 10;
    public int worldSizeY = 10;

    public int spawnDistanceFromCentre = 5;

    public bool spawnBoundary = true;
    public GameObject boundaryPrefab;

    public bool spawnSpaghettiWalls = true;
    public GameObject spaghettiWallPrefab;

    public int spagWallsToSpawnSeparate = 5;
    public int spagWallsTotalVolume = 50;
    public int spagWallDistanceFromCentre = 10;

    public GameObject corePrefab;

    public GameObject playerPrefab;

    public GameObject enemyPrefab;

    public GameObject placementDisplayPrefab;

    public WorldObjecfSpawnChance[] resourceDroppers;
    public int resourceDroppersMaxAmt = 50;

    [NonSerialized]
    public int currentResourceDroppersTotal = 0;

    public WorldObjecfSpawnChance[] initialSpawnBuildings;

    public Stat maxEnemiesToSpawn;
    [NonSerialized]
    public int currentEnemyTotal = 0;

    [NonSerialized]
    public List<Vector3> boundaryPositions = new List<Vector3>();
}
