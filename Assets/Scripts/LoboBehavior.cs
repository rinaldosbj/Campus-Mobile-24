using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoboBehavior : MonoBehaviour
{
    [SerializeField] private float wolfSpeed = 0.001f;
    private float minY;
    private float timer = 0f;
    private float timeInterval;
    void Start()
    {
        wolfSpeed = PlayerPrefs.GetFloat("wolfSpeed");
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        minY = -worldHeight*0.55f;
    }

    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position -= new Vector3(0, wolfSpeed, 0);
        if (transform.position.y < minY) {
            Destroy(gameObject);
        }
    }
}
