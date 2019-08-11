using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDropCanvas : MonoBehaviour
{
    public Text Text;
    public float TimeDuration;
    public float HeightOffset;
    public float ElevationOverTime;
    public AnimationCurve AnimationCurve;
    [Header("Set by script")]
    public int Amount;
    public Vector3 Position;

    private float start;

    private void Start()
    {
        start = Time.time;
        transform.position = Position;
        Text.text = "+" + Amount + "s";
    }

    // Update is called once per frame
    void Update()
    {
        float lerp = Mathf.InverseLerp(start, start + TimeDuration, Time.time);
        lerp = AnimationCurve.Evaluate(lerp);
        Vector3 pos = transform.position;
        pos.y = Position.y + Mathf.Lerp(0, ElevationOverTime, lerp) + HeightOffset;
        transform.position = pos;
        Color color = Text.color;
        color.a = 1 - lerp;
        Text.color = color;

        if (lerp >= 1) Destroy(gameObject);
    }
}
