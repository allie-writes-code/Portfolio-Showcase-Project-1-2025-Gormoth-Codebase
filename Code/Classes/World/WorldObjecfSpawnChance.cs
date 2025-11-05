using UnityEngine;

//! ScriptableObject class for defining world object spawn chances.
[CreateAssetMenu(fileName = "New World Object Spawn Chance", menuName = "Scriptable Objects/World/World Object Spawn Chance", order = 4)]
public class WorldObjecfSpawnChance : ScriptableObject
{
    public GameObject worldObjectPrefab;

    [SerializeField]
    private int spawnChance;
    [SerializeField]
    private float minPercentAwayFromCentreToSpawn;
    [SerializeField]
    private float maxPercentAwayFromCentreToSpawn;

    [SerializeField]
    private int howManyToSpawn;

    public bool SpawnRollPass()
    {
        bool pass = false;

        int rnd = Random.Range(1, 101);
        if (rnd <= spawnChance)
        {
            pass = true;
        }

        return pass;
    }

    public int HowManyToSpawn
    {
        get { return howManyToSpawn; }
    }

    public int MinDistance(int max)
    {
        int rmin = Mathf.RoundToInt(max * minPercentAwayFromCentreToSpawn);
        return rmin;
    }

    public int MaxDistance(int max)
    {
        int rmax = Mathf.RoundToInt(max * maxPercentAwayFromCentreToSpawn);
        return rmax;
    }
}
