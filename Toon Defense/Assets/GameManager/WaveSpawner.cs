using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WaveBurst
{
    public GameObject enemyType;
    public int count;
    public float rate;

    public WaveBurst(GameObject _enemyType, int _count, float _rate)
    {
        enemyType = _enemyType;
        count = _count;
        rate = _rate;
    }
}

[System.Serializable]
public class Wave
{
    public WaveBurst[] burst;
}

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    public enum SpawnerState { spawning, waiting, counting };
    private SpawnerState state = SpawnerState.counting;

    public Wave[] waves;
    private Dictionary<GameObject, int> enemiesCount; //Count of each type of enemy
    private Dictionary<GameObject, List<GameObject>> enemiesAvailable; //Enemies used in level

    public static List<GameObject> EnemiesAlive;

    [Header("Other")]
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    private float waveCountdown = 2f;
    private int nextWave = 0;

    public List<WayPoint> path = new List<WayPoint>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.LogError("More than one WaveSpawner in scene!");
            return;
        }

        instance = this;

        EnemiesAlive = new List<GameObject>();
        //pool
        PoolEnemies();
        waveCountdown = timeBetweenWaves;
        state = SpawnerState.counting;
        //WavesLeft();
    }

    void PoolEnemies()
    {
        enemiesCount = new Dictionary<GameObject, int>();
        enemiesAvailable = new Dictionary<GameObject, List<GameObject>>();

        foreach(Wave wave in waves)
        {
            var waveEnemiesCount = new Dictionary<GameObject, int>();
            foreach (WaveBurst burst in wave.burst)
            {
                if (waveEnemiesCount.ContainsKey(burst.enemyType))
                {
                    waveEnemiesCount[burst.enemyType] += burst.count;
                }
                else
                {
                    waveEnemiesCount.Add(burst.enemyType, burst.count);
                }
                foreach(GameObject enemy in waveEnemiesCount.Keys)
                {
                    if (enemiesCount.ContainsKey(enemy))
                    {
                        if (enemiesCount[enemy] < waveEnemiesCount[enemy])
                        {
                            enemiesCount[enemy] = waveEnemiesCount[enemy];
                        }
                    }
                    else
                    {
                        enemiesCount.Add(enemy, waveEnemiesCount[enemy]);
                    }
                }
            }
        }

        foreach (GameObject enemyType in enemiesCount.Keys)
        {
            int n = 0;
            for (int i = 0; i < enemiesCount[enemyType]; i++)
            {
                GameObject obj = (GameObject)Instantiate(enemyType, spawnPoint.position, spawnPoint.rotation);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                if (enemiesAvailable.ContainsKey(enemyType))
                {
                    enemiesAvailable[enemyType].Add(obj);
                    n++;
                }
                else
                {
                    List<GameObject> objs = new List<GameObject>();
                    objs.Add(obj);
                    enemiesAvailable.Add(enemyType, objs);
                    n = 0;
                }
            }
        }
    }

    private void Update()
    {

        //if (GameManager.GameIsOver) return;

        if (state == SpawnerState.waiting)
        {
            //check if enemies are still alive
            if (EnemiesAlive.Count <= 0)
            {
                //begin a new round
                WaveCompleted();

                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            waveCountdown = 0;
            if (state != SpawnerState.spawning)
                StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            waveCountdown = Mathf.Clamp(waveCountdown, 0f, Mathf.Infinity);
            //waveNumber.text = EnemiesAlive + " - " + waveIndex;
        }

    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        if (nextWave + 1 > waves.Length - 1)
        {
            enabled = false;
            //if (!GameManager.GameIsOver)
            //{
            //    Debug.Log("currently disabled");
            //    GameManager.instance.Winning();
            //}
        }
        else
        {
            state = SpawnerState.counting;
            waveCountdown = timeBetweenWaves;
            nextWave++;
            //WavesLeft();

        }

    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnerState.spawning;
        int count;
        float rate;
        foreach (WaveBurst enemy in _wave.burst)
        {
            count = enemy.count;
            if (count == 0)
                count = Random.Range(1, 10);
            rate = enemy.rate;
            if (rate == 0)
                rate = Random.Range(1, 5);
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy(enemy.enemyType);
                yield return new WaitForSeconds(1f / rate);
            }
        }

        state = SpawnerState.waiting;

        yield break;
    }

    void SpawnEnemy(GameObject _enemyType)
    {
        for (int i = 0; i < enemiesAvailable[_enemyType].Count; i++)
        {
            if (!enemiesAvailable[_enemyType][i].activeInHierarchy)
            {
                enemiesAvailable[_enemyType][i].transform.position = spawnPoint.position;
                enemiesAvailable[_enemyType][i].transform.rotation = spawnPoint.rotation;
                enemiesAvailable[_enemyType][i].SetActive(true);
                EnemiesAlive.Add(enemiesAvailable[_enemyType][i]);
                return;
            }
        }
    }

}
