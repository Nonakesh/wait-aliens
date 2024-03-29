﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFind;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(TargetRequester))]
public class FloorMovement : MonoBehaviour, IMovement
{
    public float MovementSpeed;
    public float RotationSpeed;
    public Point Position => GameGrid.Instance.PositionToPoint(transform.position);
    public Point Target { get; set; }

    private Health _health;
    public Health Health => _health ?? (_health = GetComponent<Health>());

    private const float Epsilon = 0.1f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public List<Point> Path { get; set; }

    private void Awake()
    {
        Path = new List<Point>();
    }    
    
    private void Update()
    {
        var hasTarget = GetComponentsInChildren<TurretBehaviour>().Any(x => x.target != null);

        if (!hasTarget)
        {
            UpdatePosition();
        }

        // Update position on grid for next frame
        var current = GameGrid.Instance.PositionToPoint(transform.position);
        GameGrid.Instance.UnitsPerTile[current.X, current.Y]++;
    }

    private void UpdatePosition()
    {
        var current = GameGrid.Instance.PositionToPoint(transform.position);

        Vector3 nextPos;
        if (current == Target)
        {
            nextPos = GameGrid.Instance.PointToPosition(current) + GetGridOffset();
        }
        else
        {
            if (current == Path[0] && Path.Count > 1)
            {
                Path.RemoveAt(0);
            }

            // If the pathfinder didn't find anything, abort
            if (Path.Count == 0)
            {
                Target = current;
                return;
            }

            var next = Path[0];
            nextPos = GameGrid.Instance.PointToPosition(next) + GetGridOffset();
        }

        Transform t = transform;

        var lookDirection = nextPos + t.forward * 0.01f - t.position;
        lookDirection.y = 0;
        targetRotation = Quaternion.LookRotation(lookDirection);

        var dist = nextPos - t.position;

        // Don't move if the distance is smaller than Epsilon
        if (dist.magnitude < Epsilon)
        {
            return;
        }

        targetPosition = t.position + Vector3.ClampMagnitude(MovementSpeed * Time.deltaTime * dist.normalized, dist.magnitude);

        t.position = targetPosition;
        t.rotation = Quaternion.Lerp(t.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        // Update position on grid for next frame
        current = GameGrid.Instance.PositionToPoint(transform.position);
        GameGrid.Instance.UnitsPerTile[current.X, current.Y]++;
    }

    private static Vector3 GetGridOffset() => GameGrid.Instance.Scale * 0.5f * new Vector3(1, 0, 1);
}