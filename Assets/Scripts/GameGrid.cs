using System;
using System.Collections.Generic;
using PathFind;
using UnityEngine;
using Grid = PathFind.Grid;

public class GameGrid : MonoBehaviour
{
    public int Width, Length;
    public float Scale;

    private Building[,] buildings;
    private Grid pathFinding;

    public static GameGrid Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        buildings = new Building[Width, Length];

        var costs = new float[Width, Length];
        for (int x = 0; x < Length; x++)
        {
            for (int z = 0; z < Length; z++)
            {
                costs[x, z] = 1;
            }
        }

        pathFinding = new Grid(Width, Length, costs);
    }

    public TileResult GetTile(Point p)
    {
        if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Length)
        {
            return new TileResult(true);
        }
        
        return new TileResult(buildings[p.X, p.Y]);
    }
    
    public void BlockTile(Building blocker, Point p)
    {
        var currentBlocker = GetTile(p);

        if (currentBlocker.IsOutside)
        {
            throw new InvalidOperationException($"The tile ({p.X}, {p.Y}) is outside the grid");
        }
        
        if (currentBlocker.Building != null)
        {
            throw new InvalidOperationException($"The tile ({p.X}, {p.Y}) is already blocked by {currentBlocker.Building.gameObject.name}");
        }

        buildings[p.X, p.Y] = blocker;
        pathFinding.Nodes[p.X, p.Y].Walkable = false;
    }

    public void UnblockTile(Building blocker, Point p)
    {
        var currentBlocker = GetTile(p);
        
        if (currentBlocker.IsOutside)
        {
            throw new InvalidOperationException($"The tile ({p.X}, {p.Y}) is outside the grid");
        }
        
        if (currentBlocker.Building != blocker)
        {
            throw new InvalidOperationException($"The tile ({p.X}, {p.Y}) is not blocked by " +
                                                $"{blocker.gameObject.name}, instead: {currentBlocker.Building.gameObject.name}");
        }

        buildings[p.X, p.Y] = null;
        pathFinding.Nodes[p.X, p.Y].Walkable = true;
    }

    public Vector3 PointToPosition(Point p)
    {
        return new Vector3(p.X, 0, p.Y) * Scale + transform.position;
    }

    public Point PositionToPoint(Vector3 position)
    {
        var pos = (position - transform.position) / Scale;
        return new Point((int) pos.x, (int) pos.z);
    }

    public List<Point> FindPath(Point start, Point end)
    {
        return Pathfinding.FindPath(pathFinding, start, end);
    }
    
    public Vector3[] GetGridLines()
    {
        var startPos = transform.position;

        var list = new List<Vector3>();
        for (int x = 0; x <= Width; x++)
        {
            list.Add(new Vector3(startPos.x + x * Scale, 0, startPos.z));
            list.Add(new Vector3(startPos.x + x * Scale, 0, startPos.z + Length * Scale));
        }

        for (int y = 0; y <= Length; y++)
        {
            list.Add(new Vector3(startPos.x, 0, startPos.z + y * Scale));
            list.Add(new Vector3(startPos.x + Width * Scale, 0, startPos.z + y * Scale));
        }

        return list.ToArray();
    }
}