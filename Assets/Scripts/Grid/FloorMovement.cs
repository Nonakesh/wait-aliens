using System.Text;
using PathFind;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(TargetRequester))]
public class FloorMovement : MonoBehaviour, IMovement
{
    public float MovementSpeed;
    public float LerpSpeed;
    public Point Target { get; set; }

    private Health _health;
    public Health Health => _health ?? (_health = GetComponent<Health>());

    private const float Epsilon = 0.1f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

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

            if (path.Count == 0)
            {
                Target = current;
                return;
            }

            var next = path[0];
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
        t.rotation = Quaternion.Lerp(t.rotation, targetRotation, LerpSpeed * Time.deltaTime);

        // Update position on grid for next frame
        current = GameGrid.Instance.PositionToPoint(transform.position);
        GameGrid.Instance.UnitsPerTile[current.X, current.Y]++;
    }

    private static Vector3 GetGridOffset() => GameGrid.Instance.Scale * 0.5f * new Vector3(1, 0, 1);
}