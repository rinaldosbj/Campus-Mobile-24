using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAssecibilityLabelUI : MonoBehaviour
{
    private AccessibleButton accessibleButton;
    [SerializeField]
    private string buttonName;
    private bool isOn;

    [SerializeField]    
    private string information;
    
    void Start()
    {
        accessibleButton = GetComponent<AccessibleButton>();
    }

    void Update()
    {
        isOn = PlayerPrefs.GetInt(information) == 1 ? true : false;
        if (isOn)
            accessibleButton.m_Text = buttonName + " , ativado";
        else
            accessibleButton.m_Text =  buttonName + " , desativado";
    }
}
