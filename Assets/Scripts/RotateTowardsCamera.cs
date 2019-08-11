using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    public bool InvertDirection = true; 
        
    void Update()
    {
        var mainCam = Camera.main.transform;
        var viewDir = (mainCam.position - transform.position) * (InvertDirection ? -1 : 1);

        transform.rotation = Quaternion.LookRotation(viewDir);
    }
}
