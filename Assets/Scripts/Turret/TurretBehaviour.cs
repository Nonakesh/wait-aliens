using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private LayerMask enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();

        //turret
        if (target == null) return;

        Vector3 forward = transform.right;
        Vector3 targetVector = target.position - transform.position;
        float angle = -Vector3.SignedAngle(forward, targetVector, Vector3.up);
        float temp = angle;
        if (angle > 0)
        {
            angle = Mathf.Min(angle, rotationSpeed * Time.deltaTime);
        }
        else
        {
            angle = Mathf.Max(angle, -rotationSpeed * Time.deltaTime);
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation *= rotation;

        //barrel
        if (barrel == null) return;
        float heightDif = transform.position.y - target.transform.position.y;
        float barrelAngle = Mathf.Asin(heightDif / targetVector.magnitude) * 180f / Mathf.PI;
        barrel.transform.localRotation = Quaternion.Euler(new Vector3(-90, -barrelAngle, 0));
    }

    void FindTarget()
    {
        var colliders = Physics.OverlapSphere(transform.position, viewDistance, enemyLayerMask.value);
        if (colliders.Length > 0)
        {
            Collider closest = null;
            float min = float.MaxValue;

            foreach (var c in colliders)
            {
                float sqr = (c.transform.position - transform.position).sqrMagnitude;
                if (sqr < min)
                {
                    min = sqr;
                    closest = c;
                }
            }
            if (closest != null)
            {
                target = closest.transform;
            }
        }
    }
}
