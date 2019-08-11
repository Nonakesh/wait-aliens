using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModel : MonoBehaviour
{
    public float Horizontal = 0.7f;
    public float Vertical = 0.7f;
    
    public float rotationSpeed = 15f;
    public Transform model;

    void Update()
    {
        var t = transform as RectTransform;

        var corners = new Vector3[4];
        t.GetWorldCorners(corners);

        var invHorizonal = 1 - Horizontal;
        var invVertical = 1 - Vertical;

        model.position = (Horizontal * Vertical * corners[0] +
                          Horizontal * invVertical * corners[1] +
                          invHorizonal * invVertical * corners[2] +
                          invHorizonal * Vertical * corners[3]);

        model.Rotate(rotationSpeed * Time.unscaledDeltaTime * Vector3.up, Space.Self);
        model.gameObject.SetActive(true);
    }
}