using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotationManager : MonoBehaviour
{
    
    public float rotationSpeed = 15f;
    public Transform myTransform;
    
    void Update()
    {
        myTransform.Rotate(rotationSpeed * Time.unscaledDeltaTime * Vector3.up, Space.Self);
        myTransform.gameObject.SetActive(true);
    }
}
