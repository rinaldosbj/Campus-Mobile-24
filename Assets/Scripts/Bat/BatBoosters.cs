using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBoosters : MonoBehaviour
{
    public AudioClip explosionClip;
    public void ExplosionBooster() {
        if (PlayerPrefs.GetInt("hasBooster") > 0) {
            PlayerPrefs.SetInt("hasBooster", 0);
            GetComponent<AudioSource>().clip = explosionClip;
            GetComponent<AudioSource>().Play();
            GameObject.Find("Main Camera").GetComponent<CameraAnimations>().ShakeCamera();
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
        }
    }
}
