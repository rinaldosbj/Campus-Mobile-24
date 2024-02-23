using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoboBehavior : MonoBehaviour
{
    [SerializeField] private float wolfSpeed = 0.001f;
    void Start()
    {
        // wolfSpeed = PlayerPrefs.GetInt("wolfSpeed"); <- in the future
    }

    void Update()
    {
        transform.position -= new Vector3(0, wolfSpeed, 0);
    }
}
