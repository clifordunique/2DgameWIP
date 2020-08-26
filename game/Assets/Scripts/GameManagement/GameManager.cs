using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    public SpawnState spawnState = SpawnState.COUNTING;

    public static GameManager instance;


    public int level;
    public static GameObject current;

    void Awake()
    {
        if (instance == null)
        {
            level = 1;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (instance != null)
        {
            Destroy(gameObject);    //gameObject is a local variable of type GameObject which is inherited from Component.                                   //It allows one to access the instance of the GameObject to which this component is attached.
        }

        Debug.Log("Init1");
        InitGame();
    }
    void Update()
    {
        if (spawnState == SpawnState.WAITING && levelActive == true)
        {
            if (!EnemiesRemain())
            {
                Debug.Log("Worked");
                levelActive = false;
                Instantiate(portal, new Vector3(5f, 1f), Quaternion.identity);
            }
        }

        else if (spawnState != SpawnState.SPAWNING && levelActive == true)
        {
            Debug.Log("NowSpawning" + GetComponent<GameManager>().level);
            StartCoroutine(SpawnWave(waves[GetComponent<GameManager>().level - 1]));
        }
        //Debug.Log(enemies.Length);

    }

    void InitGame()
    {
      SetUpLevel();
    }
    public void NextLevel()
    {
        if (level == 2)
        {
            Debug.Log("GameOver, expansion to come.");
        }
        level++;
        DestoryCurrentStage();
        SetUpLevel();
        spawnState = SpawnState.COUNTING;
        levelActive = true;
    }
    public void SetUpLevel()
    {
        currentLevel = Instantiate(levels[2], transform.position, Quaternion.identity);
    }

    public void DestoryCurrentStage()
    {
        Destroy(currentLevel);
    }

    // Update is called once per frame
    
    public float searchCountdown = 1f;

    bool EnemiesRemain()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            //this is a taxing operation, therefore not optimal to run every single frame.
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
            yield return new WaitForSeconds(1 / wave.rate);
        }
        spawnState = SpawnState.WAITING;
        yield break;
    }

    void SpawnRandomEnemy()
    {
        int random = Random.Range(0, enemies.Length);
        Instantiate(enemies[random], new Vector2(5f, 1f), Quaternion.identity);
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
