using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehaviour : MonoBehaviour
{
    public int ResourceDropPerHit = 1;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float lowerHeight;
    [SerializeField]
    private float upperHeight;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private Transform drill;

    private float currentTime;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * speed;
        UpdateDrill();
    }

    void UpdateDrill()
    {
        if(currentTime >= 1)
        {
            Harvest();
            currentTime--;
        }
        float curveValue = curve.Evaluate(currentTime);
        float height = Mathf.Lerp(lowerHeight, upperHeight, curveValue);
        drill.localPosition = Vector3.up * height;
    }

    void Harvest()
    {
        ResourceManager.AddResource(ResourceType.Time, ResourceDropPerHit);
    }
}
