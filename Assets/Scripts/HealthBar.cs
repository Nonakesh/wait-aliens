using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    [Range(0, 1)]
    public float Health;

    public Slider Slider;

    private void Update()
    {
        Slider.value = Health;
    }
}
