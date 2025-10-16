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

    public WorldObjecfSpawnChance[] resourceDroppers;
    public int resourceDroppersMinAmt = 10;
    public int resourceDroppersMaxAmt = 50;
}
