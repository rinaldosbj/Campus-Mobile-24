using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraZoom : MonoBehaviour
{
    private float velocity = 1f;
    private Vector3 velocity2;
    private float smoothTime = 0.25f;

    [SerializeField] 
    private Camera cam;
    [SerializeField] 
    private float unfocusTime = 5.5f;
    private float camOrtographicSize = 1;
    private Vector2 vector2; 
    private float normalCamOrtographicSize = 1;
    public float zoomedCamOrtographicSize = 1;

    private Vector2 minVector2 = Vector2.zero;
    private Vector2 maxVector2 = Vector2.zero;
    private bool mustFocusOnLiro;

    private void Start() {
        camOrtographicSize = normalCamOrtographicSize;
        cam.orthographicSize = normalCamOrtographicSize;

        maxVector2 = new Vector2(normalCamOrtographicSize - normalCamOrtographicSize/2 ,normalCamOrtographicSize - normalCamOrtographicSize/2);
        minVector2 = new Vector2(-normalCamOrtographicSize + normalCamOrtographicSize/2 ,-normalCamOrtographicSize + normalCamOrtographicSize/2);

        Invoke("focusOnLiro", 2f);
        Invoke("changeMustMove", 2.2f);
        Invoke("unfocus", unfocusTime);
        Invoke("callFinish", unfocusTime + 1.5f);
    }

    private void callFinish() {
        if (PlayerPrefs.GetString("NextScene") == "Win")
            FadeController.CallScene("Win");
        else
            Debug.Log(PlayerPrefs.GetString("NextScene"));
            showPopUp();
    }

    private void showPopUp() {
        MenuFunctions menuFunctions = GameObject.Find("MenuManager").GetComponent<MenuFunctions>();
        GameObject nextScenePopUp =  GameObject.Find("PopUp").transform.Find(PlayerPrefs.GetString("NextScene")).gameObject;
        menuFunctions.ConfigurationMenu = nextScenePopUp;
        menuFunctions.GoToConfiguration();
    }

    public void goToNextScene() {
        FadeController.CallScene("Scenes/Levels/"+PlayerPrefs.GetString("NextScene"));
    }

    private void Update()
    {   
        if (mustFocusOnLiro)
            vector2 = new Vector3(transform.position.x,transform.position.y);    
        
        camOrtographicSize = Mathf.Clamp(camOrtographicSize, zoomedCamOrtographicSize, normalCamOrtographicSize);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camOrtographicSize, ref velocity, smoothTime);

    }

    private void LateUpdate() {
        Vector3 vector3 = new Vector3(vector2.x, vector2.y, cam.transform.position.z);
        if (vector3.x > maxVector2.x) 
            vector3.x = maxVector2.x; 
        if (vector3.x < minVector2.x)
            vector3.x = minVector2.x;
        if (vector3.y > maxVector2.y) 
            vector3.y = maxVector2.y; 
        if (vector3.y < minVector2.y)
            vector3.y = minVector2.y; 
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, vector3, ref velocity2, smoothTime);
    }

    public void focusOnLiro() {
        if (cam != null) {
            camOrtographicSize = zoomedCamOrtographicSize;
            mustFocusOnLiro = true;
        }
    }

    public void changeMustMove() {
        FollowThePath.instance.mustMove = true;
    }

    public void unfocus() {
        if (cam != null) {
            camOrtographicSize = normalCamOrtographicSize;
            vector2 = Vector3.zero;
            mustFocusOnLiro = false;
        }
    }


}
