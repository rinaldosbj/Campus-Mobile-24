using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy shared;
    public List<string> sceneList;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if(shared == null)
        {
            shared = this;
            shared.sceneList = new List<string>();
        }

        if (shared.sceneList.Count == 0)
        {
            shared.sceneList.Add($"{gameObject.name}");
        }
        else if (shared.sceneList.Contains($"{gameObject.name}"))
        {
            Debug.Log($"Destroyed Dontdestroy -> {gameObject.name}");
            Destroy(gameObject);
        }
        else
        {
            shared.sceneList.Add($"{gameObject.name}");
        }

    }
}
