using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class GunPoint : MonoBehaviour
{
    [SerializeField] bool mouseBased;
    [SerializeField] bool targetBased;
    [SerializeField] bool limitRot;
    [SerializeField] Vector2 limitRotValues;
    [SerializeField] float rotSpeed;
    [SerializeField] bool advancedTargeting;
    ActiveTarget target;
    AIGunStats aiGun;
    GunStats gun;
    Vector3 position;
    Transform parent;
    float maxRotX;
    float maxRotY;
    private void Awake()
    {
        if (GetComponent<AIGunStats>() != null) aiGun = GetComponent<AIGunStats>();
        if (GetComponent<GunStats>() != null) gun = GetComponent<GunStats>();
        target = GetComponentInParent<ActiveTarget>();
        parent = transform.parent;
    }
    private void FixedUpdate()
    {
        if (target.target == null && !mouseBased)
        {
            transform.up = parent.up;
        }
        else
        {
            if (advancedTargeting && target.target != null && target.targetRB != null)
            {
                if (gun != null)
                {
                    position = (Vector2)target.target.position + (target.targetRB.velocity * (Vector2.Distance(transform.position, target.target.transform.position) / gun.speed));

                    //float t = Vector2.Distance(transform.position, target.target.position) / gun.speed;
                    //position = (Vector2)target.target.position + target.targetRB.velocity * t;

                    //var velocityBasedPos = (Vector2)target.target.position + target.targetRB.velocity;
                    //var speedIncluded = Vector2.LerpUnclamped(target.target.position, velocityBasedPos, 
                    //    (target.targetRB.velocity.magnitude * Vector2.Distance(transform.position, velocityBasedPos)) / gun.speed);
                    //var speedIncluded = Vector2.LerpUnclamped(target.target.position, velocityBasedPos,(target.targetRB.velocity.magnitude + (Vector2.Distance(transform.position, velocityBasedPos) - Vector2.Distance(target.target.position, velocityBasedPos))) / gun.speed);
                    //position = speedIncluded;
                    //Debug.DrawLine(transform.position, speedIncluded);
                } 
                else if (aiGun != null)
                {
                    var velocityBasedPos = (Vector2)target.target.position + target.targetRB.velocity;
                    var speedIncluded = Vector2.LerpUnclamped(target.target.position, velocityBasedPos, target.targetRB.velocity.magnitude / aiGun.speed);
                    position = speedIncluded;
                }
            }
            else if (targetBased && target.target != null)
            {
                position = target.target.position;
            }
            else if (mouseBased)
            {
                position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            Vector2 direction = (position - transform.position).normalized;

            if (limitRot)
            {
                maxRotX = limitRotValues.x + Vector2.SignedAngle(Vector2.up, parent.up);
                maxRotY = limitRotValues.y + Vector2.SignedAngle(Vector2.up, parent.up);
            }

            float unclampedAngle = Vector2.SignedAngle(Vector2.up, direction);
            float angle = Extension.ClampAngle(unclampedAngle, maxRotX, maxRotY);

            if (limitRot)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotSpeed);
            else transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, unclampedAngle), rotSpeed);
        }
        
        
        /*
        Debug.DrawRay(transform.position, new Vector2(Mathf.Cos((angle + 90) * Mathf.Deg2Rad), Mathf.Sin((angle + 90) * Mathf.Deg2Rad)).normalized);
        Debug.DrawRay(transform.position, new Vector2(Mathf.Cos((_gun.rotationAngle.x + 90 + Vector2.SignedAngle(Vector2.up, player.up)) * Mathf.Deg2Rad), Mathf.Sin((_gun.rotationAngle.x + 90 + Vector2.SignedAngle(Vector2.up, player.up)) * Mathf.Deg2Rad)).normalized);
        Debug.DrawRay(transform.position, new Vector2(Mathf.Cos((_gun.rotationAngle.y + 90 + Vector2.SignedAngle(Vector2.up, player.up)) * Mathf.Deg2Rad), Mathf.Sin((_gun.rotationAngle.y + 90 + Vector2.SignedAngle(Vector2.up, player.up)) * Mathf.Deg2Rad)).normalized);

        
        Vector3 vectorToTarget = mousePosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * _gun.rotationSpeed);
        */
    }
}
