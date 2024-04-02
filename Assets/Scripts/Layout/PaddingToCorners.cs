using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PaddingToCorners : MonoBehaviour
{
    enum ChangeRelativeTo { Leading, Botton, Top, Trailing }
    [SerializeField] private ChangeRelativeTo changeRelativeTo = ChangeRelativeTo.Top;
    [SerializeField] private float padding = 0;
    [SerializeField] private bool isRelatedToAnObject = false;
    [SerializeField] private GameObject relatedObject;
    private float timer = 0f;
    [SerializeField] private float timeInterval = 1;
    [SerializeField] private bool willAlwaysClipToTheRelatedObject;
    [SerializeField] private float speed;

    private Vector3 desiredPosition;

    [SerializeField] private Vector2 MaxPosition;
    // y = -1  max is the top of the screen
    // y = -2  max is the bottom of the screen
    // x = -1  max is the left of the screen
    // x = -2  max is the right of the screen
    // 0 is none

    void Start()
    {
        timer = timeInterval;
    }

    private void Update()
    {
        resize();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timeInterval;
            if (!willAlwaysClipToTheRelatedObject)
            {
                Destroy(gameObject.GetComponent<PaddingToCorners>());
            }
        }
    }

    void resize()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        float positionY = 0;
        float positionX = 0;

        switch (changeRelativeTo)
        {
            case ChangeRelativeTo.Top:
                positionY = worldHeight / 2 - GetComponent<Renderer>().bounds.size.y / 2 - padding;
                if (isRelatedToAnObject)
                {
                    if (relatedObject.GetComponent<Renderer>() != null)
                    {
                        positionY = relatedObject.transform.position.y - relatedObject.GetComponent<Renderer>().bounds.size.y / 2 - GetComponent<Renderer>().bounds.size.y / 2 - padding;
                    }
                    else
                    {
                        positionY = relatedObject.transform.position.y - GetComponent<Renderer>().bounds.size.y / 2 - padding;
                    }
                }
                break;
            case ChangeRelativeTo.Botton:
                positionY = -worldHeight / 2 + GetComponent<Renderer>().bounds.size.y / 2 + padding;
                if (isRelatedToAnObject)
                {
                    if (relatedObject.GetComponent<Renderer>() != null)
                    {
                        positionY = relatedObject.transform.position.y + relatedObject.GetComponent<Renderer>().bounds.size.y / 2 + GetComponent<Renderer>().bounds.size.y / 2 + padding;
                    }
                    else
                    {
                        positionY = relatedObject.transform.position.y + GetComponent<Renderer>().bounds.size.y / 2 + padding;
                    }
                }
                break;
            case ChangeRelativeTo.Leading:
                positionX = -worldWidth / 2 + GetComponent<Renderer>().bounds.size.x / 2 + padding;
                if (isRelatedToAnObject)
                {
                    if (relatedObject.GetComponent<Renderer>() != null)
                    {
                        positionX = relatedObject.transform.position.x + relatedObject.GetComponent<Renderer>().bounds.size.x / 2 + GetComponent<Renderer>().bounds.size.x / 2 + padding;
                    }
                    else
                    {
                        positionX = relatedObject.transform.position.x + GetComponent<Renderer>().bounds.size.x / 2 + padding;
                    }
                }
                break;
            case ChangeRelativeTo.Trailing:
                positionX = worldWidth / 2 - GetComponent<Renderer>().bounds.size.x / 2 - padding;
                if (isRelatedToAnObject)
                {
                    if (relatedObject.GetComponent<Renderer>() != null)
                    {
                        positionX = relatedObject.transform.position.x - relatedObject.GetComponent<Renderer>().bounds.size.x / 2 - GetComponent<Renderer>().bounds.size.x / 2 - padding;
                    }
                    else
                    {
                        positionX = relatedObject.transform.position.x - GetComponent<Renderer>().bounds.size.x / 2 - padding;
                    }
                }
                break;
        }
        if (changeRelativeTo == ChangeRelativeTo.Leading || changeRelativeTo == ChangeRelativeTo.Trailing)
        {
            desiredPosition = new Vector3(positionX, transform.position.y, transform.position.z);
        }
        else
        {
            desiredPosition = new Vector3(transform.position.x, positionY, transform.position.z);
        }

        if (MaxPosition != new Vector2(0, 0))
        {
            if (desiredPosition.x < MaxPosition.x)
            {
                desiredPosition.x = MaxPosition.x;
            }
            if (desiredPosition.y < MaxPosition.y)
            {
                desiredPosition.y = MaxPosition.y;
            }


            if (MaxPosition.y == -1)
            {
                MaxPosition.y = worldHeight / 2 - GetComponent<Renderer>().bounds.size.y / 2 - padding;
            }
            else if (MaxPosition.y == -2)
            {
                MaxPosition.y = -worldHeight / 2 + GetComponent<Renderer>().bounds.size.y / 2 - padding;
            }
            if (MaxPosition.x == -1)
            {
                MaxPosition.x = -worldWidth / 2 + GetComponent<Renderer>().bounds.size.x / 2 + padding;
            }
            else if (MaxPosition.x == -2)
            {
                MaxPosition.x = worldWidth / 2 - GetComponent<Renderer>().bounds.size.x / 2 + padding;
            }
        }

        transform.position = desiredPosition;
    }

}
