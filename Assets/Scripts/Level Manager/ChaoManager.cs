using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject groundTile;
    [SerializeField]
    private GameObject chooseAWayTile;
    [SerializeField]
    private float speed = 1.5f;
    private int currentGroundTile = 0;
    private float padding = 2f;
    private bool mustSpawn = true;
    private bool invokedTile;
    private EnemyGenerator enemyGenerator;
    private bool updatedSpeed;
    public bool isPaused;
    public bool wasPaused;

    public static ChaoManager Instance;

    void Start()
    {
        Instance = this;
        enemyGenerator = GetComponent<EnemyGenerator>();
        InvokeTile();
    }

    private void InvokeTile()
    {
        GameObject groundTileAux = groundTile;
        if (previousTile() != null)
        {
            Vector3 previousTilePosition = previousTile().transform.position;
            Bounds previousTileBounds = previousTile().GetComponent<Renderer>().bounds;
            Vector3 auxPosition = new Vector3(previousTilePosition.x, previousTileBounds.max.y + previousTileBounds.size.y / 2 - 0.055f, previousTilePosition.z);
            NameChao(groundTileAux);
            groundTileAux.GetComponent<MovingToPosition>().speed = speed;
            Instantiate(groundTileAux, auxPosition, Quaternion.identity);
        }
        else
        {
            NameChao(groundTileAux);
            groundTileAux.GetComponent<MovingToPosition>().speed = speed;
            Instantiate(groundTileAux, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private void InvokeEndingTile()
    {
        mustSpawn = false;
        GameObject groundTileAux = chooseAWayTile;

        if (previousTile() != null)
        {
            Vector3 previousTilePosition = previousTile().transform.position;
            Bounds previousTileBounds = previousTile().GetComponent<Renderer>().bounds;
            Vector3 auxPosition = new Vector3(previousTilePosition.x, previousTileBounds.max.y - 0.06f, previousTilePosition.z);
            groundTileAux.GetComponent<MovingToPosition>().speed = speed;
            Instantiate(groundTileAux, auxPosition, Quaternion.identity);
        }
        else
            Debug.Log("Error: There must be at least one tile");
    }

    private void NameChao(GameObject groundTile)
    {
        groundTile.name = "Tile" + currentGroundTile;
        currentGroundTile++;
    }

    private GameObject previousTile()
    {
        return GameObject.Find("Tile" + (currentGroundTile - 1) + "(Clone)");
    }

    private GameObject antepreviousTile()
    {
        return GameObject.Find("Tile" + (currentGroundTile - 2) + "(Clone)");
    }



    private GameObject exitTile()
    {
        return GameObject.Find(chooseAWayTile.name + "(Clone)");
    }

    private GameObject exitTileChild()
    {
        if (exitTile() != null)
        {
            if (exitTile().transform.childCount > 0)
                return exitTile().transform.GetChild(exitTile().transform.childCount - 1).gameObject;
        }
        return null;
    }

    private bool CheckIfTopWillApearOnScreen(GameObject tile)
    {
        return tile.GetComponent<Renderer>().bounds.max.y - padding < GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
    }

    private void UpdateEveryTileSpeed(float speed)
    {
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Chao"))
        {
            tile.GetComponent<MovingToPosition>().speed = speed;
        }
    }

    private void FixLastTilePosition()
    {
        if (antepreviousTile() != null)
        {
            Vector3 antepreviousTilePosition = antepreviousTile().transform.position;
            Bounds antepreviousTileBounds = antepreviousTile().GetComponent<Renderer>().bounds;
            Vector3 auxPosition = new Vector3(antepreviousTilePosition.x, antepreviousTileBounds.max.y + antepreviousTileBounds.size.y / 2 - 0.055f, antepreviousTilePosition.z);
            previousTile().transform.position = auxPosition;
        }
    }

    void Update()
    {
        if (isPaused)
        {
            wasPaused = true;
            MovingToPosition[] tiles = FindObjectsOfType<MovingToPosition>();
            if (tiles.Length > 0)
            {
                foreach (MovingToPosition tile in tiles)
                {
                    tile.speed = 0;
                }
            }
            return;
        }
        else if (wasPaused)
        {
            wasPaused = false;
            MovingToPosition[] tiles = FindObjectsOfType<MovingToPosition>();
            if (tiles.Length > 0)
            {
                foreach (MovingToPosition tile in tiles)
                {
                    tile.speed = speed;
                }
            }
        }

        if (previousTile() != null)
        {
            if (CheckIfTopWillApearOnScreen(previousTile()) && mustSpawn)
            {
                if (enemyGenerator.spawnedEveryEnemy && !enemyGenerator.isAWinningScene)
                {
                    if (invokedTile)
                        InvokeEndingTile();
                    else
                        InvokeTile();
                    invokedTile = true;
                }
                else
                {
                    InvokeTile();
                }
            }
        }



        if (exitTile() != null)
        {
            if (exitTileChild() != null)
            {
                if (exitTileChild().GetComponent<Renderer>().bounds.max.y <= GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize)
                {
                    Debug.Log("Entrou no pause do exit tile");
                    exitTile().GetComponent<MovingToPosition>().speed = 0;
                    updatedSpeed = true;
                    Time.timeScale = 1f;
                    Destroy(this);
                }
            }
        }

        if (enemyGenerator.spawnedEveryEnemy && GameObject.FindAnyObjectByType<LoboBehavior>() == null && !updatedSpeed)
        {
            UpdateEveryTileSpeed(speed * 2);
            enemyGenerator.CheckIfMustWin();
        }
    }

    private void LateUpdate()
    {
        FixLastTilePosition();
    }
}
