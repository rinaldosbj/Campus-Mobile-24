using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisclaimerTrigger : MonoBehaviour
{
    void Update()
    {
        if (PlayerPrefs.GetInt("hasSeenDisclaimer",0) == 0 && UAP_AccessibilityManager.IsEnabled()) {
            FadeController.CallScene("Disclaimer");
            PlayerPrefs.SetInt("hasSeenDisclaimer",1);
        }
        else if (PlayerPrefs.GetInt("hasSeenDisclaimer",0) != 0) {
            Destroy(gameObject.GetComponent<DisclaimerTrigger>());
        }
    }
}
