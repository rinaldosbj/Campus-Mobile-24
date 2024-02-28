using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ConfigurationMenu;

    public void StartGame() {
        SceneManager.LoadScene("Caverna");
    }

    public void GoToConfiguration() {
        MainMenu.SetActive(false);
        ConfigurationMenu.SetActive(true);
    }

    public void BackToMain() {
        MainMenu.SetActive(true);
        ConfigurationMenu.SetActive(false);
    }

    public void volume(float volume) {
        PlayerPrefs.SetFloat("volume", volume);
    }
}
