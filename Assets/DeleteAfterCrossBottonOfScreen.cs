using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterCrossBottonOfScreen : MonoBehaviour
{
    public float tolerance = 1f;
    private void FixedUpdate() {
        if (GetComponent<Renderer>().bounds.max.y + tolerance < -GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize) {
            Destroy(gameObject);
        }
    }
}
