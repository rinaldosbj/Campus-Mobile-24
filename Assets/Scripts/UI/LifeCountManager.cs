using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeCountManager : MonoBehaviour
{
    
    private TextMeshProUGUI lifeCountText;

    void Start()
    {
        lifeCountText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        lifeCountText.text = PlayerPrefs.GetInt("lifeCount").ToString();
    }
}
