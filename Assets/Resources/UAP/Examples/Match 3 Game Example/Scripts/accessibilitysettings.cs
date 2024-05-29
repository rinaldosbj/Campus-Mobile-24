using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class accessibilitysettings : MonoBehaviour
{
	public Slider m_SpeechRateSlider = null;

	void Start() {
		m_SpeechRateSlider.value = UAP_AccessibilityManager.GetSpeechRate();
	}

	public void OnSpeechRateSliderChanged()
	{
		UAP_AccessibilityManager.SetSpeechRate(Mathf.RoundToInt(m_SpeechRateSlider.value));
	}
}
