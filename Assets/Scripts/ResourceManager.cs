using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int StartTime;

    private static ResourceManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void AddResource(ResourceType type, int amount)
    {
        throw new NotImplementedException();
    }

    public static bool IsAvailable(ResourceType type, int amount)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Try to take a resource, returns true, if the requested amount was available.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static bool TryTakeResource(ResourceType type, int amount)
    {
        throw new NotImplementedException();
    }
}