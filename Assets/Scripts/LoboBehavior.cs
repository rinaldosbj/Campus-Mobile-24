using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoboBehavior : MonoBehaviour
{
    [HideInInspector]
    public float wolfSpeed;
    private float minY;
    private AudioSource audioSource;
    public int intLocation = -1;
    private float linearAdaptation = 1;
    void Start()
    {
        wolfSpeed = PlayerPrefs.GetFloat("wolfSpeed", 0.1f);
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        minY = -worldHeight * 0.55f;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position -= new Vector3(0, wolfSpeed, 0) * Time.deltaTime;
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }

        if (audioSource != null)
        {
            float aspect = (float)Screen.width / Screen.height;
            float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
            if (transform.position.y > 0)
            {
                linearAdaptation = (worldHeight / (worldHeight + transform.position.y * 2)) / (worldHeight / 4f);
            }

            audioSource.volume = linearAdaptation;
        }

        if (intLocation > GameObject.Find("Morcego").GetComponent<SwipeBat>().batState)
        {
            audioSource.panStereo = 1;
        }
        else if (intLocation < GameObject.Find("Morcego").GetComponent<SwipeBat>().batState)
        {
            audioSource.panStereo = -1;
        }
        else
        {
            audioSource.panStereo = 0;
        }
    }
}
