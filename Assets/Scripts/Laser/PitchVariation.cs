using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchVariation : MonoBehaviour
{
    public float Variation = 0.05f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch += Random.Range(-Variation, Variation);
    }
}
