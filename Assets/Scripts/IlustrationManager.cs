using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IlustrationManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTMP;
    [SerializeField]
    private Image sprite;
    [SerializeField]
    private Ilustration[] ilustrations;
    [SerializeField]
    private GameObject previousButton;
    private int currentSprite = 0;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateIlustration();
    }

    private void UpdateIlustration()
    {
        if (ilustrations == null)
            return;
        textTMP.text = ilustrations[currentSprite].text;
        sprite.sprite = ilustrations[currentSprite].sprite;
        
        if (PlayerPrefs.GetInt("AudioDescriptionIsOn") != 0)
        {
            audioSource.Stop();
            audioSource.clip = ilustrations[currentSprite].audioClip;
            audioSource.Play();
        }

        if (currentSprite == 0)
            previousButton.SetActive(false);
        else
            previousButton.SetActive(true);
    }

    public void GoToNextIlustration()
    {
        if (currentSprite + 1 < ilustrations.Length)
        {
            currentSprite++;
            UpdateIlustration();
        }
        else
            MenuFunctions.StartGame();
    }

    public void GoToPreviousIlustration()
    {
        if (currentSprite - 1 >= 0)
            currentSprite--;
        UpdateIlustration();
    }
}

[Serializable]
public class Ilustration
{
    public Sprite sprite;
    public string text;
    public AudioClip audioClip;
}