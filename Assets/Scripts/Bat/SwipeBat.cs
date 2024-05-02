using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeBat : MonoBehaviour
{
    private Vector3 fp;   // First touch position
    private Vector3 lp;   // Last touch position
    private float dragDistance;  // Minimum distance for a swipe to be registered
    private float travelWidth;   // The distance the bat will travel
    public int batState = 1;     // 0 = left, 1 = middle, 2 = right
    [SerializeField] private string leftDecisionSceneName;
    [SerializeField] private string rightDecisionSceneName;
    private bool hasMoved = false;
    private bool chosePath = false;
    private bool canMove = true;
    [HideInInspector]
    public bool canSwipe = true;
    [SerializeField] private AudioClip movedClip;
    [SerializeField] private AudioClip failedToMoveClip;

    // Double tap variables
    private float lastTapTime = 0;
    private float maxDoubleTapTime = 0.3f; // Maximum time between taps
    private int tapCount = 0;

    void Awake()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = Camera.main.orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        travelWidth = worldWidth * (1f / 3f);

        dragDistance = Screen.height * 8 / 100; // DragDistance is 8% height of the screen
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
        else if (Input.touchCount == 1 && !chosePath && canSwipe) // User is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // Get the touch

            if (touch.phase == TouchPhase.Began) // Check for the first touch
            {
                // Double tap detection
                if (Time.unscaledTime - lastTapTime < maxDoubleTapTime && tapCount == 1)
                {
                    // Double tap confirmed
                    OnDoubleTap();
                    tapCount = 0; // Reset tap count after double tap action
                }
                else
                {
                    // First tap
                    tapCount++;
                }

                lastTapTime = Time.unscaledTime;
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // Update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) // Check if the finger is removed from the screen
            {
                lp = touch.position;  // Last touch position. Omitted if you use list

                // Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    // It's a drag
                    // Reset tap count on drag
                    tapCount = 0;
                    // Check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   // If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x)) // Right swipe
                        {
                            goToTheRight();
                        }
                        else // Left swipe
                        {
                            goToTheLeft();
                        }
                    }
                }
                // else // Tap to move
                // {
                //     if (tapCount == 1)
                //     {
                //         if (touch.position.x < Screen.width / 2)
                //         {
                //             goToTheLeft();
                //         }
                //         else
                //         {
                //             goToTheRight();
                //         }
                //     }
                // }
            }
        }

        if (tapCount == 1 && (Time.unscaledTime - lastTapTime > maxDoubleTapTime))
        {
            // Timeout for double tap has passed, reset tap count
            tapCount = 0;
        }
    }

    private void OnDoubleTap()
    {
        GetComponent<BatBoosters>().ExplosionBooster();
    }

    private void goToTheRight()
    {
        if (canMove)
        {
            if (PlayerPrefs.GetInt("isOnEventState") == 1)
            {
                PlayerPrefs.SetInt("isOnEventState", 0);
                chosePath = true;
                FadeController.CallScene(rightDecisionSceneName);
            }
            if (batState < 2)
            {
                batState++;
                transform.position += new Vector3(travelWidth, 0, 0);
                MovedSucessfully();
            }
            else
            {
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
                FadeController.CallScene(leftDecisionSceneName);
            }
            if (batState > 0)
            {
                batState--;
                transform.position += new Vector3(-travelWidth, 0, 0);
                MovedSucessfully();
            }
            else
            {
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

    private void ToggleCanMove()
    {
        canMove = !canMove;
    }
}
