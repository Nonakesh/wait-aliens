using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceType Type;
    public float TickDuration = 10;
    public int ResourceDrop = 5;

    private float timeSinceLastDrop = 0;
    
    void Update()
    {
        timeSinceLastDrop += Time.deltaTime;

        if (timeSinceLastDrop >= TickDuration)
        {
            timeSinceLastDrop = 0;
            ResourceManager.AddResource(Type, ResourceDrop);
        }
    }
}