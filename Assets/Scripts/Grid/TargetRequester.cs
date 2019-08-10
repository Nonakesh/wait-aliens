using System.Linq;
using UnityEngine;

public class TargetRequester : MonoBehaviour
{
    private IMovement _movement;
    public IMovement Movement => _movement ?? (_movement = GetComponent<IMovement>());

    private void Update()
    {
        var currentPoint = GameGrid.Instance.PositionToPoint(transform.position);

        var turrets = GetComponentsInChildren<TurretBehaviour>();

        if (currentPoint == Movement.Target && turrets.All(x => x.target == null))
        {
            Movement.Target = EnemyAI.Instance.GetNewTarget();
        }
    }
}