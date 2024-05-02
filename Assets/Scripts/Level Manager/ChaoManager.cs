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

    void Start()
    {
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
            Vector3 auxPosition = new Vector3(previousTilePosition.x, previousTileBounds.max.y + previousTileBounds.size.y / 2 - 0.06f, previousTilePosition.z);
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
            Vector3 auxPosition = new Vector3(previousTilePosition.x, previousTileBounds.max.y, previousTilePosition.z);
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

    void Update()
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
                InvokeTile();
        }

        if (exitTile() != null)
        {
            if (exitTileChild() != null)
            {
                if (exitTileChild().GetComponent<Renderer>().bounds.max.y <= GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize)
                {
                    exitTile().GetComponent<MovingToPosition>().speed = 0;
                    Time.timeScale = 1f;
                    Destroy(this);
                }
            }
        }

        if (enemyGenerator.spawnedEveryEnemy && GameObject.FindAnyObjectByType<LoboBehavior>() == null && !updatedSpeed)
        {
            UpdateEveryTileSpeed(speed * 2);
            enemyGenerator.CheckIfMustWin();
            updatedSpeed = true;
        }
    }
}
