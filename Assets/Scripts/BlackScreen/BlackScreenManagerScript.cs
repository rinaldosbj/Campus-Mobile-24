using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenManagerScript : MonoBehaviour
{
    public void ToggleBlackScreen() {
        if (PlayerPrefs.GetInt("BlackScreenIsOn") == 1) {
            PlayerPrefs.SetInt("BlackScreenIsOn", 0);
        } else {
            PlayerPrefs.SetInt("BlackScreenIsOn", 1);
        }
    }
}
