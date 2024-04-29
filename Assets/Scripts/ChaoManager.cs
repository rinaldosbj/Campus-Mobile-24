using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoManager : MonoBehaviour
{
    [SerializeField] private GameObject GroundTile;
    [SerializeField] private float speed = 1.5f;
    private int currentGroundTile = 0;
    private float padding = 2f;

    void Start()
    {
        InvokeTile();
    }

    private void InvokeTile()
    {
        GameObject groundTileAux = GroundTile;
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

    private void NameChao(GameObject groundTile)
    {
        groundTile.name = "Tile" + currentGroundTile;
        currentGroundTile++;
    }

    private GameObject previousTile()
    {
        return GameObject.Find("Tile" + (currentGroundTile - 1) + "(Clone)");
    }

    private bool CheckIfTopWillApearOnScreen(GameObject tile)
    {
        return tile.GetComponent<Renderer>().bounds.max.y - padding < GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
    }

    void Update()
    {
        if (CheckIfTopWillApearOnScreen(previousTile()))
        {
            InvokeTile();
        }
    }
}
