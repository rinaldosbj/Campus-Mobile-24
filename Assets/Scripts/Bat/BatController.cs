using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            LifeManager.Instance.gotHit();
            GameObject.Find("Main Camera").GetComponent<CameraAnimations>().ShakeCamera();
            GetComponent<Animator>().SetTrigger("gotHit");
            GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Handheld.Vibrate();
        }
    }
}
