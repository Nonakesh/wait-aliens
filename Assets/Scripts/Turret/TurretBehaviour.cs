using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private const float ANGLE_THRESHOLD = 1f;

    public Health target;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private Transform[] barrelOpenings;
    private int alternateIndex;
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private LayerMask enemyLayerMask;
    [SerializeField]
    private float damage = 5;
    [SerializeField]
    private float rateOfFire;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform center;

    private float timeOfLastShot;
    private Vector3 originalRotation;

    private void Awake()
    {
        originalRotation = barrel.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();

        //turret
        if (target == null) return;

        Vector3 forward = transform.right;
        forward.y = 0;
        Vector3 targetPos = target.WorldCenter;
        targetPos.y = 0;
        Vector3 ownPos = center.position;
        ownPos.y = 0;
        Vector3 targetVector = targetPos - ownPos;

        float angle = -Vector3.SignedAngle(forward, targetVector, Vector3.up);

        if (Mathf.Abs(angle) < ANGLE_THRESHOLD)
        {
            ShootTarget();
        }

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
        float heightDif = center.position.y - target.transform.position.y;
        float barrelAngle = Mathf.Asin(heightDif / (target.WorldCenter - center.position).magnitude) * 180f / Mathf.PI;
        barrel.transform.localRotation = Quaternion.Euler(originalRotation + new Vector3(0, -barrelAngle, 0));
    }

    void FindTarget()
    {
        target = null;
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
                target = closest.GetComponent<Health>();
            }
        }
    }

    void ShootTarget()
    {
        float delayBetweenShots = 60f / rateOfFire;

        float delta = Time.time - timeOfLastShot;
        if (delta > delayBetweenShots)
        {
            Ray ray = new Ray(center.position, target.WorldCenter - center.position);
            var hits = Physics.RaycastAll(ray, viewDistance);
            var hit = FindClosestExceptSelf(hits, transform);
            if (hit != null)
            {
                if (enemyLayerMask == (enemyLayerMask | (1 << hit.transform.gameObject.layer)))
                {
                    //direct line of sight
                    //shoot

                    timeOfLastShot = Time.time;

                    var laser = Instantiate(projectilePrefab);
                    var behaviour = laser.GetComponent<ProjectileBehaviour>();

                    behaviour.from = GetBarrelOpening().position;
                    behaviour.to = target.WorldCenter;
                    float dist = (behaviour.to - behaviour.from).magnitude;
                    behaviour.duration = dist / 300f;

                    target.TakeDamage(damage);
                }
            }
        }
    }

    Transform FindClosestExceptSelf(RaycastHit[] hits, Transform self)
    {
        float min = float.MaxValue;
        Transform closest = null;

        foreach (var c in hits)
        {
            if (c.transform == self) continue;
            float sqr = (c.transform.position - self.position).sqrMagnitude;
            if (sqr < min)
            {
                min = sqr;
                closest = c.transform;
            }
        }

        return closest;
    }

    Transform GetBarrelOpening()
    {
        int len = barrelOpenings.Length;
        int index = alternateIndex++ % len;
        return barrelOpenings[index];
    }
}
