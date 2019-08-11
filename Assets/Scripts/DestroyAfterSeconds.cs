using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float DestroyTimer = 5;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(DestroyTimer);
        
        Destroy(gameObject);
    }
}
