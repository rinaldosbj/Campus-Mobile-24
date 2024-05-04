using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEventTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player entered the event trigger");
            EnemyGenerator.instance.Event();
            LeanTween.moveX(other.gameObject, 0, 0.01f);
            LeanTween.moveY(other.gameObject, 0, 2f);
        }
    }
}
