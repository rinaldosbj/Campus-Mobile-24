using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenButtonTextScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshPro;
    void Update()
    {
        if (PlayerPrefs.GetInt("BlackScreenIsOn") == 1) {
            textMeshPro.text = "Ativar Tela";
        }
        else {
            textMeshPro.text = "Desligar Tela";
        }
    }
}
