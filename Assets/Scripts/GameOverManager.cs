using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Caverna");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("Start");
    }
}
