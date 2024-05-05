using System.Collections;
using System.Collections.Generic;
// using TextSpeech;
using UnityEngine;

public class BatController : MonoBehaviour
{
    // public AudioClip picupCoinClip;


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject.Find("Main Camera").GetComponent<CameraAnimations>().ShakeCamera();
            GetComponent<Animator>().SetTrigger("gotHit");
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Handheld.Vibrate();
            LifeManager.Instance.gotHit();

            if (UAP_AccessibilityManager.IsEnabled())
            {
                // falar quantas vidas tem
                if (LifeManager.Instance.GetLifeCount() > 0)
                    StartCoroutine(UnpauseAfterDelay(3f));
                else 
                    StartCoroutine(UnpauseAfterDelay(0.035f));
            }
            else
                StartCoroutine(UnpauseAfterDelay(0.035f));

            GetComponent<SwipeBat>().canSwipe = false;
            Time.timeScale = 0f;
        }

        // if (other.gameObject.tag == "Treasure")
        // {
        //     GetComponent<AudioSource>().clip = picupCoinClip;
        //     GetComponent<AudioSource>().Play();
        //     Destroy(other.gameObject);
        //     Handheld.Vibrate();

        //     PlayerPrefs.SetInt("coinCount", PlayerPrefs.GetInt("coinCount") + 1); // TEMPORARY
        // }
    }

    IEnumerator UnpauseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UnpauseGame();
        GetComponent<SwipeBat>().canSwipe = true;
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
