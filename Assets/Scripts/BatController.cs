using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            LifeManager.Instance.gotHit();
            Destroy(other.gameObject);
        }
    }
}
