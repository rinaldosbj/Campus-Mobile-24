using System.Collections;
using System.Collections.Generic;
// using TextSpeech;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public AudioClip hitClip;
    public AudioClip picupCoinClip;


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject.Find("Main Camera").GetComponent<CameraAnimations>().ShakeCamera();
            GetComponent<Animator>().SetTrigger("gotHit");
            GetComponent<AudioSource>().clip = hitClip;
            GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Handheld.Vibrate();

            if (UAP_AccessibilityManager.IsEnabled()) {
                // falar quantas vidas tem
                StartCoroutine(UnpauseAfterDelay(1f));
            }
            else
                StartCoroutine(UnpauseAfterDelay(0.07f));
            
            GetComponent<SwipeBat>().canSwipe = false;
            Time.timeScale = 0f;
        }

        if (other.gameObject.tag == "Treasure")
        {
            GetComponent<AudioSource>().clip = picupCoinClip;
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
            Handheld.Vibrate();

            PlayerPrefs.SetInt("coinCount", PlayerPrefs.GetInt("coinCount") + 1); // TEMPORARY
        }
    }

    IEnumerator UnpauseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UnpauseGame();
        GetComponent<SwipeBat>().canSwipe = true;
        LifeManager.Instance.gotHit();
    }

    void UnpauseGame() {
        Time.timeScale = 1f;
    }
}
