using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Building))]
public class EnemySpawner : MonoBehaviour
{
    public EnemyWave[] Waves;

    public float DifficultyIncreaseTime = 60;

    private ILookup<int, EnemyWave> waveDictionary;
    private int maxDifficulty;
    
    private int difficulty;

    private EnemyWave currentWave;
    
    private float difficultyTiming;
    private float waveTiming;

    private Building _building;
    public Building Building => _building ?? (_building = GetComponent<Building>());
    
    private void Awake()
    {
        maxDifficulty = Waves.Max(x => x.Difficulty);
        waveDictionary = Waves.ToLookup(x => x.Difficulty, x => x);
    }
    
    private void Update()
    {
        difficultyTiming += Time.deltaTime;
        waveTiming += Time.deltaTime;

        // Increase difficulty every DifficultyIncreaseTime seconds
        if (difficulty < maxDifficulty && difficultyTiming >= DifficultyIncreaseTime)
        {
            difficulty++;
            difficultyTiming = 0;
        }

        // If no wave is currently selected, randomly select a new one
        if (currentWave == null)
        {
            // Get a wave from the current difficulty and clone it
            var waves = waveDictionary[difficulty].ToArray();
            currentWave = new EnemyWave(waves[Random.Range(0, waves.Length)]);
            
            // Set the wave time directly, to immediately spawn the first enemies
            waveTiming = currentWave.SpawnTime;
        }

        // After the cooldown replace the wave with a new one
        if (currentWave.Enemies.Count == 0)
        {
            if (waveTiming >= currentWave.WaveCooldown)
            {
                currentWave = null;
                waveTiming = 0;
            }
            return;
        }
        
        // Spawn enemies
        if (waveTiming >= currentWave.SpawnTime)
        {
            waveTiming = 0;

            var pos = transform.position;
            var width = Building.Width * GameGrid.Instance.Scale;
            var length = Building.Length * GameGrid.Instance.Scale;
            
            for (int i = 0; i < currentWave.SpawnAmount; i++)
            {
                if (currentWave.Enemies.Count == 0)
                {
                    return;
                }
                
                var x = Random.Range(pos.x, pos.x + width);
                var z = Random.Range(pos.z, pos.z + length);
                var spawnPos = new Vector3(x, 0, z);

                var enemyIndex = Random.Range(0, currentWave.Enemies.Count);
                var enemy = currentWave.Enemies[enemyIndex];
                currentWave.Enemies.RemoveAt(enemyIndex);

                // Spawn
                var instance = Instantiate(enemy, spawnPos, Quaternion.identity).GetComponent<IMovement>();
                instance.Target = GameGrid.Instance.PositionToPoint(spawnPos);
                EnemyAI.Instance.Units.Add(instance);
            }
        }
    }
}

[Serializable]
public class EnemyWave
{
    [Tooltip("Higher is harder")]
    public int Difficulty = 0;
    
    public List<Health> Enemies;
    
    [Tooltip("The time between enemy spawns for this wave")]
    public float SpawnTime;
    
    [Tooltip("The amount of enemies spawned per spawn-tick (Until the wave is completely spawned)")]
    public int SpawnAmount;

    [Tooltip("After each wave there is a short cooldown before the next one starts.")]
    public float WaveCooldown;
    
    public EnemyWave(EnemyWave wave)
    {
        Difficulty = wave.Difficulty;
        Enemies = wave.Enemies.ToList();
        SpawnTime = wave.SpawnTime;
        SpawnAmount = wave.SpawnAmount;
        WaveCooldown = wave.WaveCooldown;
    }
}
