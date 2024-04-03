using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public AudioClip hitClip;
    public AudioClip picupCoinClip;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            LifeManager.Instance.gotHit();
            GameObject.Find("Main Camera").GetComponent<CameraAnimations>().ShakeCamera();
            GetComponent<Animator>().SetTrigger("gotHit");
            GetComponent<AudioSource>().clip = hitClip;
            GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Handheld.Vibrate();
        }

        if (other.gameObject.tag == "Treasure")
        {
            GetComponent<AudioSource>().clip = picupCoinClip;
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
            Handheld.Vibrate();

            PlayerPrefs.SetInt("coinCount", PlayerPrefs.GetInt("coinCount") + 1); // TEMPORARY
            print(PlayerPrefs.GetInt("coinCount"));
        }
    }
}
