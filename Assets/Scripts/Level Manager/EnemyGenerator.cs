using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private int difficultyLevel = 0;
    [SerializeField] private int enemiesNumber;
    private int enemiesCount = 0;
    private float timer = 0f;
    private float timeInterval;
    [SerializeField] private List<GameObject> enemies;
    private bool canSpawn = true;
    private bool triggeredEvent = false;
    [SerializeField] private bool isAWinningScene;

    private AudioSource audioSource;

    private void Start()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;

        PlayerPrefs.SetFloat("wolfSpeed", 0.08f);

        if (transform.Find("Audio Source") != null) {
            audioSource = transform.Find("Audio Source").GetComponent<AudioSource>();
        }

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
        }

        SpawnRandomEnemy();

        timer = timeInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && canSpawn)
        {
            timer = timeInterval;
            SpawnRandomEnemy();
            enemiesCount++;
            if (enemiesCount == enemiesNumber)
            {
                canSpawn = false;
            }
        }

        if (!canSpawn && !triggeredEvent)
        {
            triggeredEvent = true;
            Invoke("Event", 7f);
        }
    }

    private void Event()
    { 
        if (isAWinningScene)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            PlayerPrefs.SetInt("isOnEventState", 1);
            if (transform.Find("Audio Source") != null) {
                audioSource.Play();
            }
        }
    }

    private void SpawnRandomEnemy()
    {
        System.Random random = new System.Random();
        int randomInt = random.Next(0, enemies.Count);

        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;

        List<Vector3> possibleSpawnLocations = new List<Vector3>();

        int batState = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;

        possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f));
        possibleSpawnLocations.Add(new Vector3(0, worldHeight * 1.01f));
        possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f));

        switch (batState) {
            case 0:
                possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f));
                break;
            case 1:
                possibleSpawnLocations.Add(new Vector3(0, worldHeight * 1.01f));
                break;
            case 2:
                possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f));
                break;
            default:
                break;
        }

        int randomInt2 = random.Next(0, 4);
        Vector3 randomLocation = possibleSpawnLocations[randomInt2];
        transform.position = randomLocation;

        GameObject enemyBoy = enemies[randomInt];
        enemyBoy.GetComponent<LoboBehavior>().intLocation = randomInt2;

        Instantiate(enemies[randomInt], transform.position, transform.rotation);
    }
}
