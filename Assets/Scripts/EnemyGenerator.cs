using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private int difficultyLevel = 0;
    private float timer = 0f;
    private float timeInterval;
    [SerializeField] private List<GameObject> enemies;

    private void Start() {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;

        switch (difficultyLevel) {
            case 1:
                timeInterval = 3f + worldHeight/3;
                break;
            case 2:
                timeInterval = 2.4f + worldHeight/3;
                break;
            case 3:
                timeInterval = 2f + worldHeight/3;
                break;
        }

        SpawnRandomEnemy();


        timer = timeInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) 
        {
            timer = timeInterval;

            SpawnRandomEnemy();
        }
    }

    private void SpawnRandomEnemy() {
        System.Random random = new System.Random();
            int randomInt = random.Next(0,enemies.Count);

            float aspect = (float)Screen.width / Screen.height;
            float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
            float worldWidth = worldHeight * aspect;

            List<Vector3> possibleSpawnLocations = new List<Vector3>();

            possibleSpawnLocations.Add(new Vector3((worldWidth/6)*2,worldHeight*1.01f));
            possibleSpawnLocations.Add(new Vector3(0,worldHeight*1.01f));
            possibleSpawnLocations.Add(new Vector3((worldWidth/6)*-2,worldHeight*1.01f));

            int randomInt2 = random.Next(0,3);
            Vector3 randomLocation = possibleSpawnLocations[randomInt2];
            transform.position = randomLocation;

            Instantiate(enemies[randomInt],transform.position, transform.rotation);
    }
}
