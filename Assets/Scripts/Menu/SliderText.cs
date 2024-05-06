using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        text.text = value.ToString();
    }
}
