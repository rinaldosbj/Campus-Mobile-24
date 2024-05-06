using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideInVoiceOver : MonoBehaviour
{
    [SerializeField]
    private GameObject shownObject;
    [SerializeField]
    private bool mustShowObject;
    void Update()
    {
        if (UAP_AccessibilityManager.IsEnabled())
        {
            if (mustShowObject)
            {
                if (shownObject != null)
                {
                    shownObject.SetActive(true);
                }
            }
            else
                gameObject.SetActive(false);
        }
    }
}
