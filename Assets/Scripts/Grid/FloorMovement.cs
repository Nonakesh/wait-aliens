using System.Text;
using PathFind;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    [Header("Unit Settings")]
    public float MovementSpeed;
    
    [Header("Runtime variables")]
    public Point Target;

    private const float Epsilon = 0.1f;
    
    private void Update()
    {
        var current = GameGrid.Instance.PositionToPoint(transform.position);

        Vector3 nextPos;
        if (current == Target)
        {
            nextPos = GameGrid.Instance.PointToPosition(current) + GetGridOffset();
        }
        else
        {
            var path = GameGrid.Instance.FindPath(current, Target);

            var next = path[0];
            nextPos = GameGrid.Instance.PointToPosition(next) + GetGridOffset();
        }
        
        Transform t = transform;
        t.LookAt(nextPos);
        
        var dist = nextPos - t.position;

        // Don't move if the distance is smaller than Epsilon
        if (dist.magnitude < Epsilon)
        {
            return;
        }
        
        t.position += Vector3.ClampMagnitude(MovementSpeed * Time.deltaTime * dist.normalized, dist.magnitude);
    }

    private static Vector3 GetGridOffset() => GameGrid.Instance.Scale * 0.5f * Vector3.one;
}
