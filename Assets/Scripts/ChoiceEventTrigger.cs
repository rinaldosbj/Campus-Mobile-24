using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEventTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player entered the event trigger");
            EnemyGenerator.instance.Event();
            LeanTween.moveX(other.gameObject, 0, 0.1f);
            LeanTween.moveY(other.gameObject, 0, 2f);
            GameObject.Find("Morcego").GetComponent<AudioSource>().clip = GameObject.Find("Morcego").GetComponent<SwipeBat>().movedClip;
            GameObject.Find("Morcego").GetComponent<AudioSource>().Play();
        }
    }
}
