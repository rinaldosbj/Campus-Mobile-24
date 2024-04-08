using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenIsOnScript : MonoBehaviour
{
    public GameObject blackScreen;
    void Update()
    {
        if (PlayerPrefs.GetInt("BlackScreenIsOn") == 1) {
            blackScreen.SetActive(true);
        }
        else {
            blackScreen.SetActive(false);
        }
    }
}
