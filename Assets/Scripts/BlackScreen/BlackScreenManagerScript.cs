using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenManagerScript : MonoBehaviour
{

    [SerializeField]
    private Sprite onImage;
    [SerializeField]
    private Sprite offImage;
    private bool isOn;

    [SerializeField]    
    private string information;

    void Start()
    {
        if (PlayerPrefs.GetInt(information) == 1) {
            isOn = true;
        }
    }

    void UpdateUI() 
    {
        if (isOn)
        {
            // Mostrar On
            GetComponent<Image>().sprite = onImage;
        }
        else
        {
            // Mostrar Off
            GetComponent<Image>().sprite = offImage;
        }
    }

    public void ToggleBlackScreen() {
        isOn =!isOn;
        PlayerPrefs.SetInt(information, isOn? 1 : 0);
        UpdateUI();
    }
}
