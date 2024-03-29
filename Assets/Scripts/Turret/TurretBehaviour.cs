﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private const float ANGLE_THRESHOLD = 5f;

    public Health target;
    
    public float Damage = 5;
    public float RateOfFire;
    public float RotationSpeed;
    public float ViewDistance;
    
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private Transform[] barrelOpenings;
    private int alternateIndex;
    
    [SerializeField]
    private LayerMask enemyLayerMask;
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
            angle = Mathf.Min(angle, RotationSpeed * Time.deltaTime);
        }
        else
        {
            angle = Mathf.Max(angle, -RotationSpeed * Time.deltaTime);
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation *= rotation;

        //barrel
        if (barrel == null) return;
        float heightDif = center.position.y - target.WorldCenter.y;
        float barrelAngle = Mathf.Asin(heightDif / (target.WorldCenter - center.position).magnitude) * 180f / Mathf.PI;
        barrel.transform.localRotation = Quaternion.Euler(originalRotation + new Vector3(0, -barrelAngle, 0));
    }

    void FindTarget()
    {
        target = null;
        var colliders = Physics.OverlapSphere(transform.position, ViewDistance, enemyLayerMask.value);
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
        float delayBetweenShots = 60f / RateOfFire;

        float delta = Time.time - timeOfLastShot;
        if (delta > delayBetweenShots)
        {
            var barrelPos = GetBarrelOpening().position;
            
            Ray ray = new Ray(barrelPos, target.WorldCenter - barrelPos);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            var hits = Physics.RaycastAll(ray, ViewDistance);
            var hit = FindClosestExceptSelf(hits, transform);
            if (hit != null)
            {
                if ((enemyLayerMask & (1 << hit.gameObject.layer)) > 0)
                {
                    //direct line of sight
                    //shoot

                    timeOfLastShot = Time.time;

                    var laser = Instantiate(projectilePrefab);
                    var behaviour = laser.GetComponent<ProjectileBehaviour>();

                    behaviour.from = barrelPos;
                    behaviour.to = target.WorldCenter;
                    float dist = (behaviour.to - behaviour.from).magnitude;
                    behaviour.duration = dist / 300f;

                    target.TakeDamage(Damage);
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
            float sqr = (c.point - self.position).sqrMagnitude;
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
        alternateIndex = (alternateIndex + 1) % len;
        return barrelOpenings[alternateIndex];
    }
}
