using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTrack : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SoundVolume", 70));
    }

    public void SetVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        Refreshslider(_value);
        PlayerPrefs.SetFloat("SoundVolume", _value);
        GetComponent<AudioSource>().volume = _value/100;
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
