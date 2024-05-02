using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideInVoiceOver : MonoBehaviour
{
    void Update()
    {
        if (UAP_AccessibilityManager.IsEnabled())
        {
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().enabled = false;
        }
    }
}
