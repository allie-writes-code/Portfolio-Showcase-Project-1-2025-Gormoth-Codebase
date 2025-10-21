using System.Collections;
using UnityEngine;

//! MonoBehaviour class for controlling enemy spawns in game.
public class EnemySpawnManager : MonoBehaviour
{
    private bool spawningActive = false;

    [SerializeField]
    private DelegateListener enemyDiedListener;

    [SerializeField]
    private DelegateListener levelLoadedListener;

    [SerializeField]
    private WorldSpawnData spawnData;

    [SerializeField]
    private Stat waveTimeStat;
    [SerializeField]
    private float waveTimer = 0;

    private void Update()
    {
        if (!spawningActive) return;

        if (waveTimer < waveTimeStat.Value)
        {
            waveTimer += Time.deltaTime;
        }
        else
        {
            waveTimer = 0;
            StartCoroutine("SpawnWave");
        }
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log("Spawning wave");

        for(int i = 0; i < spawnData.maxEnemiesToSpawn.Value; i++)
        {
            GameObject newEnemy = Instantiate(spawnData.enemyPrefab, spawnData.boundaryPositions[Random.Range(0, spawnData.boundaryPositions.Count)], spawnData.enemyPrefab.transform.rotation);
            newEnemy.name = "Enemy";
            spawnData.currentEnemyTotal++;
        }

        yield return null;
    }

    private void OnEnable()
    {
        enemyDiedListener.RegisterFunction(EnemyDied);
        levelLoadedListener.RegisterFunction(BeginSpawningEnemies);
    }

    private void OnDisable()
    {
        enemyDiedListener.DeregisterFunction(EnemyDied);
        levelLoadedListener.DeregisterFunction(BeginSpawningEnemies);
    }

    private void BeginSpawningEnemies()
    {
        spawningActive = true;
    }

    private void EnemyDied()
    {
        spawnData.currentEnemyTotal--;
    }
}
