using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimations : MonoBehaviour
{
    [SerializeField]
    private GameObject container;
    private Transform cameraTrans;
    private Vector3 initialPosition;
    private float worldHeight;

    private void Start()
    {
        cameraTrans = Camera.main.transform;
        initialPosition = cameraTrans.position;
        worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
    }

    public void ShakeCamera() {
        moveUp();
    }

    private void moveUp() {
        LeanTween.moveY(container, worldHeight/55, 0.01f).setOnComplete(moveDown);
    }
    private void moveDown() {
        LeanTween.moveY(container,-worldHeight/55, 0.05f).setOnComplete(DefaultGamePosition);
    }

    private void DefaultGamePosition() {
        LeanTween.move(container, initialPosition, 0.1f);
        Debug.Log("Shaked");
    }
}
