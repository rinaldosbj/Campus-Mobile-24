using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    public GameObject ConfigurationMenu;
    [SerializeField] private bool mustPause;

    private void Awake() {
        UAP_AccessibilityManager.RegisterOnThreeFingerDoubleTapCallback(ToggleState);
        if (PlayerPrefs.GetInt("AudioDescriptionIsOn",0) == 0 && PlayerPrefs.GetInt("enteredAQUI",0) == 0) {
            Debug.Log("Entrooouuu");
            PlayerPrefs.SetInt("AudioDescriptionIsOn",1);
            PlayerPrefs.SetInt("enteredAQUI",1);
        }
    }

    public static void StartGame()
    {
        PlayerPrefs.SetInt("isOnEventState", 0);
        PlayerPrefs.SetInt("lifeCount", 4);
        PlayerPrefs.SetInt("coinCount", 0);
        PlayerPrefs.SetString("NextScene", "Caverna");
        FadeController.CallScene("Pre-Caverna");
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

    public void ToggleState() {
        if (PlayerPrefs.GetInt("isOnTutorial") == 1)
            return;

        if (MainMenu != null && ConfigurationMenu != null) {
            if (MainMenu.activeSelf == true) {
                GoToConfiguration();
            }
            else {
                BackToMain();
            }
        }
    }

    public void LoadMenuScene()
    {
        Time.timeScale = 1;
        FadeController.CallScene("Start");
    }

    public void LoadContextoScene(bool isComingFoward)
    {
        if (isComingFoward) {
            if (PlayerPrefs.GetInt("AudioDescriptionIsOn") == 0 && UAP_AccessibilityManager.IsEnabled())
            {
                if (isComingFoward)
                    StartGame();
                else
                    LoadPaperScene();
            }
            else 
            {
                FadeController.CallScene("Contexto");
            }
        }
    }

    public void LoadPaperScene()
    {
        FadeController.CallScene("Capitulos");
    }

    public void LoadMapaScene() 
    {
        Time.timeScale = 1;
        var currentSceneName = "Scenes/Mapa/Pre-"+PlayerPrefs.GetString("NextScene");
        if (PlayerPrefs.GetString("NextScene") != "Caverna")
            currentSceneName = "Scenes/Mapa/Caverna"; // TEMPORARY
            
        FadeController.CallScene(currentSceneName);
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
