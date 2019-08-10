using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int StartTime = 0;
    public int StartScore = 0;
    
    private Dictionary<ResourceType, int> resources;
    private static ResourceManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void AddResource(ResourceType type, int amount)
    {
        _instance.resources[type] += amount;
    }

    public static bool IsAvailable(ResourceType type, int amount)
    {
        return _instance.resources[type] >= amount;
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
        return RemoveResource(type, amount);
    }

    private static bool RemoveResource(ResourceType type, int amount)
    {
        if (IsAvailable(type, amount))
        {
            _instance.resources[type] -= amount;
            return true;
        }
        
        return false;
    }
}