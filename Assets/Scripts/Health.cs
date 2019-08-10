using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Vector3 Center;

    public Vector3 WorldCenter => t.TransformPoint(Center);

    public float MaxHealth;
    public float CurrentHealth { get; set; }
    public float RegenerationPerSecond;
    public ResourceDrop[] Drops;

    public Transform DeathEffect;

    private Transform t;

    private void Awake()
    {
        t = transform;
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        CheckDeathState();
        
        Regenerate();
    }
    
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
    }
    
    private void DropResources()
    {
        foreach (var drop in Drops)
        {
            ResourceManager.AddResource(drop.Type, drop.Amount);
        }
    }

    private void Regenerate()
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + RegenerationPerSecond * Time.deltaTime);
    }

    private void CheckDeathState()
    {
        if (CurrentHealth <= 0)
        {
            DropResources();

            if (DeathEffect != null)
            {
                var t = transform;
                Instantiate(DeathEffect, t.position, t.rotation);
            }

            // Unblock the grid on death
            var building = GetComponent<Building>();
            if (building != null)
            {
                building.UnblockAll();
            }
            
            Destroy(gameObject);
        }
    }
}
