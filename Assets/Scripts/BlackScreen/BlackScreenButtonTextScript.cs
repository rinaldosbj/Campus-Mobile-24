using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenButtonTextScript : MonoBehaviour
{
    void Update()
    {
        if (PlayerPrefs.GetInt("BlackScreenIsOn") == 1) {
            GetComponent<TextMeshProUGUI>().text = "Ativar Tela";
        }
        else {
            GetComponent<TextMeshProUGUI>().text = "Desligar Tela";
        }
    }
}
