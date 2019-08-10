using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public Vector3 from;
    public Vector3 to;
    public float duration;

    private float start;

    private void Awake()
    {
        start = Time.time;
    }

    private void Start()
    {
        transform.position = from;
    }

    private void Update()
    {
        float delta = Time.time - start;
        if (delta > duration * 2) Destroy(gameObject);
        float lerp = Mathf.InverseLerp(0, duration, delta);
        transform.position = Vector3.Lerp(from, to, lerp);
    }
}
