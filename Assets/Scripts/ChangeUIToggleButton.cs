using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUIToggleButton : MonoBehaviour
{
    [SerializeField]
    private Sprite onImage;
    [SerializeField]
    private Sprite offImage;
    private bool isOn;

    [SerializeField]    
    private string information;
    [SerializeField]
    private bool mustCheckOnUpdate;


    void Start()
    {
        isOn = PlayerPrefs.GetInt(information) == 1 ? true : false;
        UpdateUI();
    }

    public void ChangeUIToggle() 
    {
        isOn =!isOn;
        PlayerPrefs.SetInt(information, isOn? 1 : 0);
        Debug.Log(PlayerPrefs.GetInt(information));
        Debug.Log(isOn);
        UpdateUI();
    }

    private void UpdateUI() 
    {
        if (isOn)
        {
            if (onImage != null)
                GetComponent<Image>().sprite = onImage;
        }
        else
        {
            if (offImage != null)
                GetComponent<Image>().sprite = offImage;
        }
    }

    void Update() {
        if (mustCheckOnUpdate) 
        {
            isOn = PlayerPrefs.GetInt(information) == 1 ? true : false;
            UpdateUI();
        }
    }
}
