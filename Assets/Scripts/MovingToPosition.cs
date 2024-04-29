using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToPosition : MonoBehaviour
{
    [SerializeField] private Vector2 position;
    [SerializeField] public float speed = 0.1f;
    private Vector3 targetPosition;
    [SerializeField] private bool willDestroyOnEnd;
    [SerializeField] private bool willMoveInDirection;
    enum MoveRelativeTo { Both, X, Y }
    [SerializeField] private MoveRelativeTo moveRelativeTo;
    [SerializeField] private bool isNegative;
    [SerializeField] private Vector2 maxPosition;

    void Start()
    {
        targetPosition = new Vector3(position.x, position.y, transform.position.z);
    }

    void Update() {
        if (willMoveInDirection) {
            switch (moveRelativeTo) {
                case MoveRelativeTo.X:
                    if (isNegative) {
                        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
                    }
                    else {
                        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                    }
                    break;
                case MoveRelativeTo.Y:
                    if (isNegative) {
                        transform.position += new Vector3(0,-speed * Time.deltaTime, 0);
                    }
                    else {
                        transform.position += new Vector3(0,speed * Time.deltaTime, 0);
                    }
                    break;
                case MoveRelativeTo.Both:
                    if (isNegative) {
                        transform.position += new Vector3(-speed * Time.deltaTime,-speed * Time.deltaTime, 0);
                    }
                    else {
                        transform.position += new Vector3(speed * Time.deltaTime,speed * Time.deltaTime, 0);
                    }
                    break;
            }
        }
        else {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (transform.position == targetPosition)
        {
            if (willDestroyOnEnd)
            {
                Destroy(gameObject);
            }
            this.enabled = false;
        }
    }
}
