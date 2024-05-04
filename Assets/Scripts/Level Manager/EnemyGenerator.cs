using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    public bool isInTutorial;
    [SerializeField] 
    private int difficultyLevel = 0;
    [SerializeField] 
    private int enemiesNumber;
    private int enemiesCount = 0;
    private float timer = 0f;
    private float timeInterval;
    [SerializeField] 
    private List<GameObject> enemies;
    [SerializeField] 
    private GameObject coin;
    private bool canSpawn = true;
    private bool canSpawnCoin = true;
    private bool triggeredEvent = false;
    [SerializeField] 
    public bool isAWinningScene;

    private AudioSource audioSource;

    private float aspect;
    private float worldHeight;
    private float worldWidth;
    [HideInInspector]
    public bool spawnedEveryEnemy;
    public string winningSceneName;
    public static EnemyGenerator instance;
    private bool calledScene;

    private void Start()
    {
        EnemyGenerator.instance = this;
        aspect = (float)Screen.width / Screen.height;
        worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        worldWidth = worldHeight * aspect;

        SetDifficulty();

        if (transform.Find("Audio Description") != null)
        {
            audioSource = transform.Find("Audio Description").GetComponent<AudioSource>();
        }

        if (!isInTutorial)
        {
            SpawnRandomEnemy();
        }

        timer = timeInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && canSpawn && !isInTutorial)
        {
            timer = timeInterval;
            canSpawnCoin = true;
            SpawnRandomEnemy();
            enemiesCount++;
            if (enemiesCount == enemiesNumber)
            {
                canSpawn = false;
                canSpawnCoin = false;
                spawnedEveryEnemy = true;
                Debug.Log("Spawned every enemy");
            }
        }
        else if (timer <= timeInterval / 2 && canSpawnCoin && !isInTutorial)
        {
            canSpawnCoin = false;
            //SpawnCoin();
        }
    }

    public void Event()
    {
            PlayerPrefs.SetInt("isOnEventState", 1);
            if (transform.Find("Audio Description") != null)
            {
                audioSource.Play();
            }
    }

    public void CheckIfMustWin() {
        if (isAWinningScene && !calledScene)
        {
            calledScene = true;
            var currentScene = PlayerPrefs.GetString("NextScene");
            Debug.Log("CheckIfMustWin"+currentScene);
            FadeController.CallScene("Scenes/Mapa/"+currentScene);
            PlayerPrefs.SetString("NextScene", "Win");
        }
    }

    private void SetDifficulty()
    {
        switch (difficultyLevel)
        {
            case 1:
                timeInterval = 3f + worldHeight / 4;
                PlayerPrefs.SetFloat("wolfSpeed", 1.75f);
                break;
            case 2:
                timeInterval = 2f + worldHeight / 4;
                PlayerPrefs.SetFloat("wolfSpeed", 3f);
                break;
            case 3:
                timeInterval = 1f + worldHeight / 4;
                PlayerPrefs.SetFloat("wolfSpeed", 5f);
                break;
            case 4:
                timeInterval = worldHeight / 4;
                PlayerPrefs.SetFloat("wolfSpeed", 7f);
                break;
            case 100:
                PlayerPrefs.SetFloat("wolfSpeed", 7f);
                timeInterval = 1.2f;
                break;
            case 999:
                PlayerPrefs.SetFloat("wolfSpeed", 7f);
                timeInterval = 0.5f;
                break;
        }
    }

    private void SpawnCoin()
    {
        System.Random random = new System.Random();
        List<Vector3> possibleSpawnLocations = new List<Vector3> {
            new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f),
            new Vector3(0, worldHeight * 1.01f),
            new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f)
        };

        int randomInt = random.Next(0, 3);
        Vector3 randomLocation = possibleSpawnLocations[randomInt];

        if (random.Next(0, 4) == 0)
        {
            Instantiate(coin, randomLocation, transform.rotation);
        }
    }

    private void SpawnRandomEnemy()
    {
        SetDifficulty();

        System.Random random = new System.Random();
        int randomInt = random.Next(0, enemies.Count);

        List<Vector3> possibleSpawnLocations = new List<Vector3> {
            new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f),
            new Vector3(0, worldHeight * 1.01f),
            new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f)
        };

        int batState = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;

        int randomInt2 = random.Next(0, 4);

        if (randomInt2 == 3)
            randomInt2 = batState;

        Vector3 randomLocation = possibleSpawnLocations[randomInt2];

        transform.position = randomLocation;

        GameObject enemyBoy = enemies[randomInt];
        enemyBoy.GetComponent<LoboBehavior>().intLocation = randomInt2;
        Instantiate(enemyBoy, transform.position, transform.rotation);
    }
}
