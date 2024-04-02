using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private int heartEquivalent = 0;
    void Update()
    {
        if (LifeManager.Instance.lifeCount >= heartEquivalent) {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}