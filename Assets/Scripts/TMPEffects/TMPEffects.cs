using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EffectType { None, Wobble, Shake, WobbleAndShake, Wave }


public class TMPEffect : MonoBehaviour
{
    [SerializeField] private EffectType effectType = EffectType.None;
    [SerializeField, Range(0.1f, 10f)] private float intensity = 1f;
    [SerializeField] private bool willFadeIn;
    [SerializeField, Range(0.1f, 10f)] private float showSpeed = 1f;
    [SerializeField] private GameObject animateAfter;
    [SerializeField] private bool isColorChanging;
    [SerializeField] private Gradient rainbow;
    [SerializeField] private bool animateOnlyEspecificStrings;
    [SerializeField] private SpecificStringEffect[] stringsToAnimate;
    [SerializeField] private float colorVelocity = 1f;

    private TMP_Text textMesh;
    private Mesh mesh;
    private Vector3[] vertices;
    private bool animationCompleted = false;
    private float timeBetweenLetters;
    private float lastLetterTime = 0f;
    private bool canRunAnimation = true;
    private int currentCharacter = 0;

    private void Start()
    {
        timeBetweenLetters = 0.2f / showSpeed;

        textMesh = GetComponent<TMP_Text>();
        if (willFadeIn)
            textMesh.maxVisibleCharacters = 0;
        else
            animationCompleted = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ClearSpecificStrings();
        }
        if (animateAfter != null && animateAfter.GetComponent<TMPEffect>() != null)
        {
            canRunAnimation = animateAfter.GetComponent<TMPEffect>().animationCompleted;
        }

        if (willFadeIn && !animationCompleted && canRunAnimation)
        {
            if (Time.time - lastLetterTime > timeBetweenLetters && currentCharacter < textMesh.text.Length)
            {
                while (textMesh.text[currentCharacter] == ' ')
                {
                    currentCharacter++;
                    if (currentCharacter >= textMesh.text.Length)
                    {
                        animationCompleted = true;
                        break;
                    }
                }

                textMesh.maxVisibleCharacters = currentCharacter + 1;
                currentCharacter++;
                lastLetterTime = Time.time;
            }
            else if (currentCharacter >= textMesh.text.Length)
            {
                Invoke("AnimationCompleted", timeBetweenLetters);
            }
        }
        else
        {
            UpdateTextMesh();
        }
    }

    void AnimationCompleted()
    {
        animationCompleted = true;
    }

    void UpdateTextMesh()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        Color[] colors = mesh.colors;

        if (animateOnlyEspecificStrings)
        {
            ApplyEffectsToSpecificStrings();
        }
        else
        {
            ApplyGlobalEffects();
        }

        if (isColorChanging)
        {
            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[i];
                int index = charInfo.vertexIndex;

                colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time * colorVelocity + vertices[index].x * intensity * 0.001f, 1f));
                colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time * colorVelocity + vertices[index + 1].x * intensity * 0.001f, 1f));
                colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time * colorVelocity + vertices[index + 2].x * intensity * 0.001f, 1f));
                colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time * colorVelocity + vertices[index + 3].x * intensity * 0.001f, 1f));
            }
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    void ApplyEffectToCharacter(EffectType type, int charIndex, float intensity)
    {
        switch (type)
        {
            case EffectType.Shake:
                ApplyShakeEffect(charIndex, intensity);
                break;
            case EffectType.Wobble:
                ApplyWobbleEffect(charIndex, intensity);
                break;
            case EffectType.WobbleAndShake:
                ApplyWobbleAndShakeEffect(charIndex, intensity);
                break;
            case EffectType.Wave:
                ApplyWaveEffect(charIndex, intensity);
                break;
            default:
                break;
        }
    }

    void ApplyEffectsToSpecificStrings()
    {
        foreach (var specificString in stringsToAnimate)
        {
            int startIndex = textMesh.text.IndexOf(specificString.text);
            while (startIndex != -1)
            {
                int endIndex = startIndex + specificString.text.Length;
                for (int i = startIndex; i < endIndex; i++)
                {
                    ApplyEffectToCharacter(specificString.effectType, i, specificString.intensity);
                }
                startIndex = textMesh.text.IndexOf(specificString.text, startIndex + 1);
            }
        }
    }

    void ApplyGlobalEffects()
    {
        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            ApplyEffectToCharacter(effectType, i, intensity);
        }
    }



    void ApplyShakeEffect(int charIndex, float intensity)
    {
        TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[charIndex];
        int index = charInfo.vertexIndex;
        Vector3 offset = Shake(Time.time + charIndex, intensity);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;
    }

    void ApplyWobbleEffect(int charIndex, float intensity)
    {
        TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[charIndex];
        int index = charInfo.vertexIndex;
        Vector3 offset = Wobble(Time.time + charIndex, intensity);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;
    }

    void ApplyWobbleAndShakeEffect(int charIndex, float intensity)
    {
        TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[charIndex];
        int index = charInfo.vertexIndex;
        Vector3 offset = Wobble(Time.time + charIndex, intensity);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;

        offset = Shake(Time.time + charIndex, intensity * 0.5f);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;
    }

    void ApplyWaveEffect(int charIndex, float intensity)
    {
        TMP_CharacterInfo charInfo = textMesh.textInfo.characterInfo[charIndex];
        int index = charInfo.vertexIndex;
        Vector3 offset = Wave(Time.time + charIndex, intensity);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;
    }


    Vector2 Wobble(float time, float intensity)
    {
        float x = Mathf.Sin(time * 10f * intensity);
        float y = Mathf.Cos(time * 10f * intensity);
        return new Vector2(x, y);
    }

    Vector2 Shake(float time, float intensity)
    {
        float x = Mathf.Sin(time * UnityEngine.Random.Range(3.0f, 10.0f) * intensity);
        float y = Mathf.Cos(time * UnityEngine.Random.Range(3.0f, 10.0f) * intensity);
        return new Vector2(x, y);
    }

    Vector2 Wave(float time, float intensity)
    {
        float speed = 10f * intensity / 1.5f;
        float y = Mathf.Sin(time * speed) * speed * 1.5f;
        return new Vector2(0, y);
    }


    public void ResetEffect()
    {
        if (willFadeIn)
        {
            textMesh.maxVisibleCharacters = 0;
            animationCompleted = false;
        }
        currentCharacter = 0;
    }

    public void ClearSpecificStrings(){
        stringsToAnimate = new SpecificStringEffect[0];
    }

    public void AddSpecificString(string text, EffectType effectType, float intensity) {
        var specificString = new SpecificStringEffect();
        specificString.text = text;
        specificString.effectType = effectType;
        specificString.intensity = intensity;
        Array.Resize(ref stringsToAnimate, stringsToAnimate.Length + 1);
        stringsToAnimate[stringsToAnimate.Length - 1] = specificString;
    }
}


[Serializable]
public class SpecificStringEffect
{
    public string text;
    public EffectType effectType;
    public float intensity = 1f;
}

#if UNITY_EDITOR
[CustomEditor(typeof(TMPEffect))]
public class TMPEffectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var isColorChangingProperty = serializedObject.FindProperty("isColorChanging");
        var showRainbowProperty = serializedObject.FindProperty("rainbow");
        var effectTypeProperty = serializedObject.FindProperty("effectType");
        var intensityProperty = serializedObject.FindProperty("intensity");
        var willFadeInProperty = serializedObject.FindProperty("willFadeIn");
        var showSpeedProperty = serializedObject.FindProperty("showSpeed");
        var animateAfterProperty = serializedObject.FindProperty("animateAfter");
        var animateOnlyEspecificStringsProperty = serializedObject.FindProperty("animateOnlyEspecificStrings");
        var stringsToAnimateProperty = serializedObject.FindProperty("stringsToAnimate");
        var colorVelocityProperty = serializedObject.FindProperty("colorVelocity");

        EditorGUILayout.PropertyField(animateOnlyEspecificStringsProperty);

        if (!animateOnlyEspecificStringsProperty.boolValue)
        {
            EditorGUILayout.PropertyField(effectTypeProperty);
        }
        else
        {
            EditorGUILayout.PropertyField(stringsToAnimateProperty);
            effectTypeProperty.enumValueIndex = (int)EffectType.None;
        }

        if (effectTypeProperty.enumValueIndex != (int)EffectType.None)
        {
            EditorGUILayout.PropertyField(intensityProperty);
        }
        else
        {
            intensityProperty.floatValue = 1;
        }

        EditorGUILayout.PropertyField(willFadeInProperty);

        if (willFadeInProperty.boolValue)
        {
            EditorGUILayout.PropertyField(animateAfterProperty);
            EditorGUILayout.PropertyField(showSpeedProperty);
        }

        EditorGUILayout.PropertyField(isColorChangingProperty);

        if (isColorChangingProperty.boolValue)
        {
            EditorGUILayout.PropertyField(showRainbowProperty);
            EditorGUILayout.PropertyField(colorVelocityProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif


