using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ConfigurationMenu;
    [SerializeField] private bool mustPause;

    public void StartGame()
    {
        PlayerPrefs.SetInt("isOnEventState", 0);
        PlayerPrefs.SetInt("lifeCount", 4);
        PlayerPrefs.SetInt("coinCount", 0);
        FadeController.CallScene("Caverna");
    }

    public void GoToConfiguration()
    {
        if (MainMenu != null)
            MainMenu.SetActive(false);
        if (ConfigurationMenu != null)
            ConfigurationMenu.SetActive(true);
        if (mustPause)
        {
            Time.timeScale = 0;
            PauseSounds();
        }
    }

    public void BackToMain()
    {
        if (MainMenu != null)
            MainMenu.SetActive(true);
        if (ConfigurationMenu != null)
            ConfigurationMenu.SetActive(false);
        if (mustPause)
        {
            Time.timeScale = 1;
            UnpauseSounds();
        }
    }

    public void LoadMenuScene()
    {
        FadeController.CallScene("Start");
    }

    public void LoadContextoScene()
    {
        FadeController.CallScene("Contexto");
    }

    private void PauseSounds() {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
            {
                if (!(audio.gameObject.name == "SoundManager" || audio.gameObject.name == "Morcego" || audio.gameObject.name == "Audio Description"))
                {
                    audio.Pause();
                }
            }
    }

    private void UnpauseSounds() {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
            {
                if (!(audio.gameObject.name == "SoundManager" || audio.gameObject.name == "Morcego" || audio.gameObject.name == "Audio Description"))
                {
                    audio.Play();
                }
            }
    }
}
