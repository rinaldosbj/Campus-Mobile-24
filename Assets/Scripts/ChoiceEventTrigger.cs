using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEventTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player entered the event trigger");
            EnemyGenerator.instance.Event();
        }
    }
}
