using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingRadar : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;


    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}
