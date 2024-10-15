using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateTime(float elapsedTime, float generationTime)
    {
        // Ensure the slider's value is clamped between 0 and 1
        slider.maxValue = generationTime;
        slider.value = elapsedTime;
    }

    public void ResetBar()
    {
        slider.value = 0; // Reset the slider value
    }

    void Start()
    {
        ResetBar(); // Optionally reset the bar at start
    }
}
