using System;
using System.Collections.Generic;
using PathFind;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public int TargetDistance = 2;
    
    public List<IMovement> Units = new List<IMovement>();

    public static EnemyAI Instance { get; private set; }

    private List<Point> PossibleTargets = new List<Point>();

    private bool updateRequested = false;
    
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

    public Point GetNewTarget()
    {
        if (PossibleTargets.Count == 0)
        {
            return null;
        }
        
        return PossibleTargets[Random.Range(0, PossibleTargets.Count)];
    }

    private void LateUpdate()
    {
        if (updateRequested)
        {
            UpdateGrid();
            updateRequested = false;
        }
    }

    private void UpdateGrid()
    {
        PossibleTargets.Clear();
        
        // Find all reachable tiles that are directly neighboring enemies
        var list = new Queue<Point>();
        var reachable = new bool[GameGrid.Instance.Width, GameGrid.Instance.Length];
        var enemyNeighbor = new List<Point>();
        list.Enqueue(GameGrid.Instance.PositionToPoint(transform.position));

        while (list.Count > 0)
        {
            var point = list.Dequeue();
            
            // Skip if this point was visited before
            if (reachable[point.X, point.Y])
            {
                continue;
            }
            
            reachable[point.X, point.Y] = true;

            bool isEnemyNeighbor = false;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var newPoint = new Point(point.X + x, point.Y + y);

                    // Check if the point is on the grid and free
                    var tileResult = GameGrid.Instance.GetTile(newPoint);

                    if (tileResult.IsOutside)
                    {
                        continue;
                    }
                    
                    // Check if the tile is next to a player building
                    if (!tileResult.IsWalkable)
                    {
                        if (tileResult.Building != null)
                        {
                            isEnemyNeighbor = true;
                        }
                        
                        continue;
                    }
                    
                    list.Enqueue(newPoint);
                }
            }
            
            if (isEnemyNeighbor)
            {
                enemyNeighbor.Add(point);
            }
        }
        
        // Dilute target list
        var dilutionVisited = new bool[GameGrid.Instance.Width, GameGrid.Instance.Length];
        foreach (var point in enemyNeighbor)
        {
            for (int x = -TargetDistance; x <= TargetDistance; x++)
            {
                for (int y = -TargetDistance; y <= TargetDistance; y++)
                {
                    // Ignore points that are out of range
                    if (new Vector3(x, y).sqrMagnitude > TargetDistance * TargetDistance)
                    {
                        continue;
                    }
                    
                    var p = new Point(point.X + x, point.Y + y);
                    
                    // Check out of bounds
                    if (p.X < 0 || p.X >= GameGrid.Instance.Width || p.Y < 0 || p.Y >= GameGrid.Instance.Length)
                    {
                        continue;
                    }
                    
                    // Check if the point is reachable
                    if (!reachable[p.X, p.Y])
                    {
                        continue;
                    }

                    // Check if we've already added this point
                    if (dilutionVisited[p.X, p.Y])
                    {
                        continue;
                    }

                    PossibleTargets.Add(point);
                    dilutionVisited[p.X, p.Y] = true;
                }
            }
        }
    }
    
    public void RequestGridUpdate()
    {
        updateRequested = true;
    }
}
