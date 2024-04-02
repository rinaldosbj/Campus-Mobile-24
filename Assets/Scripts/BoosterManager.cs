using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public bool willHaveBooster;
    private void Start() {
        if (willHaveBooster) 
        {
            PlayerPrefs.SetInt("hasBooster", 1);
        }
        else {
            PlayerPrefs.SetInt("hasBooster", 0);
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("hasBooster") == 1) {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
