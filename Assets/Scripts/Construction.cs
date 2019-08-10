using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Construction : MonoBehaviour
{
    public float BuildingTime;

    public Health FinalBuilding;

    private Health _health;
    public Health Health => _health ?? (_health = GetComponent<Health>());

    private float buildRate;
    
    private void Start()
    {
        Health.CurrentHealth = 1;
        Health.MaxHealth = FinalBuilding.MaxHealth;
        
        buildRate = FinalBuilding.MaxHealth / BuildingTime;
    }
    
    // Update is called once per frame
    private void Update()
    {
        Health.CurrentHealth += buildRate * Time.deltaTime;
        
        if (Health.CurrentHealth >= FinalBuilding.MaxHealth)
        {
            var t = transform;
            Instantiate(FinalBuilding, t.position, t.rotation);
            Destroy(gameObject);
        }
    }
}
