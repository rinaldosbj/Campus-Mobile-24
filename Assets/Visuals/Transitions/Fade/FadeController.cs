using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static Animator animator;
    private static string nextScene;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public static void CallScene(string scene)
    {
        Debug.Log($"Calling scene: {scene}");
        nextScene = scene;
        animator.SetTrigger("FadeOut");
    }

    public void FadeOutCompleted()
    {
        SceneManager.LoadScene(nextScene);
    }
}
