using UnityEngine;

//! ScriptableObject class for defining world object spawn chances.
[CreateAssetMenu(fileName = "New World Object Spawn Chance", menuName = "Scriptable Objects/World/World Object Spawn Chance", order = 4)]
public class WorldObjecfSpawnChance : ScriptableObject
{
    public GameObject worldObjectPrefab;

    [SerializeField]
    private int spawnChance;
    [SerializeField]
    private float percentAwayFromCentreToSpawn;

    public bool SpawnRollPass()
    {
        bool pass = false;

        int rnd = Random.Range(0, 101);
        if (rnd <= spawnChance)
        {
            pass = true;
        }

        return pass;
    }

    public int MinDistance(int max)
    {
        return Mathf.RoundToInt(max * percentAwayFromCentreToSpawn);
    }
}
