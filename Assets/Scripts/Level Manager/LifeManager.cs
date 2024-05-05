using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    private int lifeCount;
    static public LifeManager Instance { get; private set; }
    [SerializeField]
    private AudioSource descriptionSource;
    [SerializeField]
    private AudioSource hitSource;
    [SerializeField]
    private AudioWithInt[] audioHitDescription;
    [SerializeField]
    private AudioClip[] hitClips;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lifeCount = PlayerPrefs.GetInt("lifeCount");
    }

    public void AddLife()
    {
        lifeCount++;
        PlayerPrefs.SetInt("lifeCount", lifeCount);
    }

    public void gotHit()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
            PlayerPrefs.SetInt("lifeCount", lifeCount);
            if (UAP_AccessibilityManager.IsEnabled())
                foreach (AudioWithInt audioWithInt in audioHitDescription)
                {
                    if (audioWithInt.number == lifeCount)
                    {
                        descriptionSource.clip = audioWithInt.audio;
                        descriptionSource.Play();
                    }
                }
            
            if (hitClips.Length > 0) 
            {
                AudioClip randomClip = hitClips[UnityEngine.Random.Range(0, hitClips.Length)];
                hitSource.clip = randomClip;
                hitSource.Play();
            }
        }
        if (lifeCount == 0)
        {
            FadeController.CallScene("GameOver");
        }
    }

    public int GetLifeCount()
    {
        return lifeCount;
    }
}

[Serializable]
public class AudioWithInt
{
    public int number;
    public AudioClip audio;
}