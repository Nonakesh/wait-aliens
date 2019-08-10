using System;
using System.Collections.Generic;
using PathFind;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int Width;
    public int Length;

    public bool IsWalkable = false;
    
    private bool isInitialized = false;
    
    public void Start()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        var startPoint = GameGrid.Instance.PositionToPoint(transform.position);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
            {
                var point = new Point(x + startPoint.X, y + startPoint.Y);
                
                BlockTile(point, IsWalkable);
            }
        }
        
        // Align current transform, just to be save
        var newPos = GameGrid.Instance.PointToPosition(startPoint);
        transform.position = newPos;
        
        isInitialized = true;
    }

    private readonly List<Point> blockedTiles = new List<Point>();

    public void BlockTile(Point point, bool isWalkable)
    {
        blockedTiles.Add(point);

        GameGrid.Instance.BlockTile(this, point, isWalkable);
    }

    // This one will probably not be needed.. I thought I would use this for moving units as well, but that would be too much work...
    public void UnblockTile(Point point)
    {
        blockedTiles.RemoveAll(p => p == point);

        GameGrid.Instance.UnblockTile(this, point);
    }

    public void UnblockAll()
    {
        foreach (var tile in blockedTiles)
        {
            GameGrid.Instance.UnblockTile(this, tile);
        }
    }
}