using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;
    private int SFXGamb = 0;
    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 75));
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

        if (SFXGamb > 0) {
        GameObject.Find("SFX").GetComponent<AudioSource>().Play();
        }

        SFXGamb++;
    }

    public void Refreshslider(float _value)
    {
        soundSlider.value = _value;
    }
}
