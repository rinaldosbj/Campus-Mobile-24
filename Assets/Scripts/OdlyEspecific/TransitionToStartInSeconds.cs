using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToStartInSeconds : MonoBehaviour
{
    public float startInSeconds = 4;
    void Start()
    {
        Invoke("Transition", startInSeconds);
    }

    private void Transition() 
    {
        FadeController.CallScene("Start");
    }

}
