using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    private int lifeCount;
    static public LifeManager Instance { get; private set; }

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        lifeCount = PlayerPrefs.GetInt("lifeCount");
    }

    public void AddLife() {
        lifeCount++;
        PlayerPrefs.SetInt("lifeCount", lifeCount);
    }

    public void gotHit() {
        if (lifeCount > 0) {
            lifeCount--;
            PlayerPrefs.SetInt("lifeCount", lifeCount);
        }
        if (lifeCount == 0) {
            SceneManager.LoadScene("GameOver");
        }
    }

    public int GetLifeCount() {
        return lifeCount;
    }
}
