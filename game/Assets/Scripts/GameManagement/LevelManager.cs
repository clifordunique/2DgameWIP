using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    public List<GameObject> Monsters = new List<GameObject>();

    public GameObject currentLevel;

    //private int currentLevelNumber;


    public bool levelActive = true;
    public int monstersRemaining;
    public GameObject portal;

    //spawnLogic
    [System.Serializable]
    public class Wave
    {
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public GameObject[] enemies;     //0:ogre, 1:Necromancer, 2:Goblin

    public enum SpawnState { SPAWNING, WAITING, COUNTING}

    public SpawnState spawnState = SpawnState.COUNTING;


    public void SetUpLevel(int level)
    {        
        currentLevel = Instantiate(levels[level-1], transform.position, Quaternion.identity);
    }

    public void DestoryCurrentStage() {
        Destroy(currentLevel);
    }
    void Start()
    {
        Debug.Log(GetComponent<GameManager>().level);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnState == SpawnState.WAITING  && levelActive == true)
        {
            if (!EnemiesRemain())
            {
                Debug.Log("Worked");
                levelActive = false;
                Instantiate(portal, new Vector3(5f, 5f), Quaternion.identity);
            }

        }

        else if( spawnState != SpawnState.SPAWNING && levelActive == true)
        {
            Debug.Log("NowSpawning" + GetComponent<GameManager>().level);
            StartCoroutine(SpawnWave(waves[GetComponent<GameManager>().level - 1]));           
        }
        //Debug.Log(enemies.Length);

    }
    public float searchCountdown = 1f;

    bool EnemiesRemain()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave wave)
    {
        spawnState = SpawnState.SPAWNING;

        //spawnwave
        Debug.Log("LOOP");
        for (int i = 0; i < wave.count; i++)
        {
            
            SpawnRandomEnemy();
            yield return new WaitForSeconds(1/wave.rate);
        }
            spawnState = SpawnState.WAITING;
        yield break;
    }

    void SpawnRandomEnemy()
    {
        int random = Random.Range(0, enemies.Length);
        Instantiate(enemies[random], new Vector2 (5f,5f), Quaternion.identity);
    }
    public GameObject SpawnOgre()
    {
        return Instantiate(enemies[0]);
    }
    public GameObject SpawnNecromancer()
    {
        return Instantiate(enemies[1]);
    }
    public GameObject SpawnGoblin()
    {
        return Instantiate(enemies[2]);
    }
}
