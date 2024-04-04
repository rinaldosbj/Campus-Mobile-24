using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Moedas Coletadas: " + PlayerPrefs.GetInt("coinCount").ToString();
    }
}
