using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int StartTime = 0;
    public int StartScore = 0;
    
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    private static ResourceManager _instance;

    private void Awake()
    {
        _instance = this;

        resources[ResourceType.Time] = StartTime;
        resources[ResourceType.Score] = StartScore;
    }

    public static void AddResource(ResourceType type, int amount)
    {
        _instance.resources[type] += amount;
    }

    public static void AddResources(ResourceDrop[] drops)
    {
        foreach (var drop in drops)
        {
            AddResource(drop.Type, drop.Amount);
        }
    }

    public static int GetResource(ResourceType type)
    {
        return _instance.resources[type];
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
        if (IsAvailable(type, amount))
        {
            _instance.resources[type] -= amount;
            return true;
        }
        
        return false;
    }

    public static bool TryTakeResources(ResourceDrop[] costs)
    {
        if (costs.All(x => IsAvailable(x.Type, x.Amount)))
        {
            foreach (var cost in costs)
            {
                _instance.resources[cost.Type] -= cost.Amount;
            }
            return true;
        }

        return false;
    }
}