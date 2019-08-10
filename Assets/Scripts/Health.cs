using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    public float RegenerationPerSecond;
    public float RegenerationTick = 1;
    public ResourceDrop[] Drops;

    public Transform DeathEffect;
    
    private float timeSinceLastRegeneration;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        timeSinceLastRegeneration += Time.deltaTime;
        if (timeSinceLastRegeneration >= RegenerationTick)
        {
            timeSinceLastRegeneration = 0;
            CurrentHealth += RegenerationPerSecond;
        }
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
