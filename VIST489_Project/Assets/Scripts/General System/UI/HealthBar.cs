using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fillImage;

    [SerializeField] float decreaseSpeed = 2f;


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fillImage.color = gradient.Evaluate(1f);

        
        

    }

    public void SetHealth(float health)
    {
        StartCoroutine(SmoothTransition(health));
    }

    IEnumerator SmoothTransition(float target)
    {
        while (slider.value > target)
        {
            slider.value = Mathf.MoveTowards(slider.value, target, Time.deltaTime * decreaseSpeed);
            yield return null;
        }

        slider.value = Mathf.Floor(target);
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }
}
