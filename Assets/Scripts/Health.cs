﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public HealthBar HealthBar;
    
    public Vector3 Center;

    public Vector3 WorldCenter => t.TransformPoint(Center);

    public float MaxHealth;
    public float CurrentHealth { get; set; }
    public float RegenerationPerSecond;
    public ResourceDrop[] Drops;

    public Transform DeathEffect;

    public bool DeathTriggersGameOver = false;
    
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

        if (HealthBar != null)
        {
            HealthBar.gameObject.SetActive(CurrentHealth < MaxHealth - 0.01f);
            HealthBar.Health = CurrentHealth / MaxHealth;
        }
    }
    
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
    }
    
    private void DropResources()
    {
        foreach (var drop in Drops)
        {
            drop.Position = t.position;
            ResourceManager.AddResource(drop.Type, drop.Amount, drop.Position);
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
                Instantiate(DeathEffect, t.transform.TransformPoint(Center), t.rotation);
            }

            // Unblock the grid on death
            var building = GetComponent<Building>();
            if (building != null)
            {
                building.UnblockAll();
            }

            if (DeathTriggersGameOver)
            {
                SceneManager.LoadScene("GameOver");
            }
            
            Destroy(gameObject);
        }
    }
}
