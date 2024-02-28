using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;
    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 70));
    }
    public void SetVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        Refreshslider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetVoLumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void Refreshslider(float _value)
    {
        soundSlider.value = _value;
    }
}
