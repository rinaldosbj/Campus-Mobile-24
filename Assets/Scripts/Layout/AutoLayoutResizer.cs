using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLayout : MonoBehaviour
{
    [SerializeField] private float screenProportionX = 1f;
    [SerializeField] private float screenProportionY = 1f;

    enum ChangeRelativeTo { Both, X, Y }
    [SerializeField] private bool isSincronized = false;
    [SerializeField] private ChangeRelativeTo changeRelativeTo = ChangeRelativeTo.Both;

    void Start()
    {
        newScale(gameObject);
    }

    public void newScale(GameObject theGameObject)
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;

        Vector3 size = theGameObject.GetComponent<Renderer>().bounds.size;
        Vector3 rescale = theGameObject.transform.localScale;

        if (changeRelativeTo == ChangeRelativeTo.Y || changeRelativeTo == ChangeRelativeTo.Both)
        {
            rescale.y = (worldHeight * rescale.y * screenProportionY) / size.y;
        }
        if (changeRelativeTo == ChangeRelativeTo.X || changeRelativeTo == ChangeRelativeTo.Both)
        {
            rescale.x = (worldWidth * rescale.x * screenProportionX) / size.x;
        }

        if (isSincronized && changeRelativeTo != ChangeRelativeTo.Both)
        {
            if (changeRelativeTo == ChangeRelativeTo.Y)
            {
                rescale.x = rescale.y;
            }
            else
            {
                rescale.y = rescale.x;
            }
        }

        theGameObject.transform.localScale = rescale;
    }
}
