using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] public int lifeCount;
    static public LifeManager Instance { get; private set; }

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        // lifeCount = PlayerPrefs.GetInt("lifeCount"); <- in the future
    }

    public void AddLife() {
        lifeCount++;
    }

    public void gotHit() {
        if (lifeCount > 0) {
            lifeCount--;
        }
        if (lifeCount == 0) {
            Debug.Log("MORCEGO MORREU");
        }
    }
}
