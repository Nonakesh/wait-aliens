using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<IMovement> Units = new List<IMovement>();

    public static EnemyAI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("There is more than one instance of EnemyAI in this scene.");
        }
        
        Instance = this;
    }

    private void Update()
    {
        // Remove all dead units from the list
        Units.RemoveAll(x => x == null);
    }
}
