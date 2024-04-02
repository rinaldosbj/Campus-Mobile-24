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

    private void Start()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;

        PlayerPrefs.SetFloat("wolfSpeed", 0.08f);

        switch (difficultyLevel)
        {
            case 1:
                timeInterval = 3f + worldHeight / 3;
                break;
            case 2:
                timeInterval = 2f + worldHeight / 3;
                break;
            case 3:
                timeInterval = 2f + worldHeight / 3;
                PlayerPrefs.SetFloat("wolfSpeed", 0.15f);
                break;
            case 4:
                timeInterval = worldHeight / 3;
                PlayerPrefs.SetFloat("wolfSpeed", 0.17f);
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
        if (GameObject.Find("PathDivider") != null)
        {
            if (GameObject.Find("PathDivider").GetComponent<PaddingToCorners>() != null)
            {
                GameObject.Find("PathDivider").GetComponent<PaddingToCorners>().enabled = true;
            }
        }
        if (isAWinningScene)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            PlayerPrefs.SetInt("isOnEventState", 1);
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

        possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f));
        possibleSpawnLocations.Add(new Vector3(0, worldHeight * 1.01f));
        possibleSpawnLocations.Add(new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f));

        int randomInt2 = random.Next(0, 3);
        Vector3 randomLocation = possibleSpawnLocations[randomInt2];
        transform.position = randomLocation;

        GameObject enemyBoy = enemies[randomInt];
        enemyBoy.GetComponent<LoboBehavior>().intLocation = randomInt2;

        Instantiate(enemies[randomInt], transform.position, transform.rotation);
    }
}
