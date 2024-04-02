using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeBat : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private float travelWidth;   //the distance the bat will travel
    public int batState = 1;   //0 = left, 1 = middle, 2 = right
    [SerializeField] private string leftDecitionSceneName;
    [SerializeField] private string rightDecitionSceneName;
    private bool hasMoved = false;
    private bool chosePath = false;
    private bool canMove = true;
    [SerializeField] private AudioClip movedClip;
    [SerializeField] private AudioClip failedToMoveClip;


    void Start()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        travelWidth = worldWidth * (1f / 3f);

        dragDistance = Screen.height * 8 / 100; //dragDistance is 8% height of the screen
    }

    void Update()
    {

        if ((PlayerPrefs.GetInt("isOnEventState") == 1 && !hasMoved) || chosePath)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            hasMoved = true;
            ToggleCanMove();
            Invoke("ToggleCanMove", 5f);
        }

        else if (Input.touchCount == 1 && !chosePath) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))
                        {   //Right swipe
                            goToTheRight();
                        }
                        else
                        {   //Left swipe
                            goToTheLeft();
                        }
                    }
                }
                else
                {
                    if (touch.position.x < Screen.width / 2)
                    {
                        goToTheLeft();
                    }
                    else
                    {
                        goToTheRight();
                    }
                }
            }
        }
    }

    private void goToTheRight()
    {
        if (canMove)
        {
            if (PlayerPrefs.GetInt("isOnEventState") == 1)
            {
                PlayerPrefs.SetInt("isOnEventState", 0);
                chosePath = true;
                SceneManager.LoadScene(rightDecitionSceneName);
            }
            if (batState < 2)
            {
                batState++;
                transform.position += new Vector3(travelWidth, 0, 0);
                MovedSucessfully();
            }
            else {
                FailedToMove();
            }
        }
    }

    private void goToTheLeft()
    {
        if (canMove)
        {
            if (PlayerPrefs.GetInt("isOnEventState") == 1)
            {
                PlayerPrefs.SetInt("isOnEventState", 0);
                chosePath = true;
                SceneManager.LoadScene(leftDecitionSceneName);
            }
            if (batState > 0)
            {
                batState--;
                transform.position += new Vector3(-travelWidth, 0, 0);
                MovedSucessfully();
            }
            else {
                FailedToMove();
            }
        }
    }

    private void MovedSucessfully() 
    {
        GetComponent<AudioSource>().clip = movedClip;
        GetComponent<AudioSource>().Play();
    }

    private void FailedToMove() 
    {
        GetComponent<AudioSource>().clip = failedToMoveClip;
        GetComponent<AudioSource>().Play();
    }

    private void ToggleCanMove() {
        canMove = !canMove;
    }
}
