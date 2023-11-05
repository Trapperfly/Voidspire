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
    [SerializeField] bool notAGun;
    [SerializeField] bool noFireWhenOutOfReach;
    public bool doRotate = true;
    ActiveTarget target;
    AIGunStats aiGun;
    GunStats gun;
    Transform bsp;
    Vector3 position;
    Transform parent;
    float maxRotX;
    float maxRotY;
    private void Awake()
    {
        if (!notAGun)
        {
            if (GetComponent<AIGunStats>() != null) aiGun = GetComponent<AIGunStats>();
            if (GetComponent<GunStats>() != null) gun = GetComponent<GunStats>();
            foreach (Transform child in transform)
            {
                if (child.name == "BulletSpawnPoint") bsp = child.transform;
            }
            target = GetComponentInParent<ActiveTarget>();
            parent = transform.parent;
        }
        else
        {
            if (GetComponent<AIGunStats>() != null) aiGun = GetComponent<AIGunStats>();
            if (GetComponent<GunStats>() != null) gun = GetComponent<GunStats>();
            foreach (Transform child in transform)
            {
                if (child.name == "BulletSpawnPoint") bsp = child.transform;
            }
            target = GetComponent<ActiveTarget>();
        }
    }
    private void FixedUpdate()
    {
        if (target.target == null && !mouseBased)
        {
            if (!notAGun)
            {
                position = transform.parent.position + transform.parent.transform.up * 10;
                Debug.DrawLine(transform.position, position);
            }
        }
        else
        {
            if (advancedTargeting && target.target != null && target.targetRB != null)
            {
                if (gun != null)
                {
                    position = (Vector2)target.target.position 
                        + (target.targetRB.velocity 
                        * (Vector2.Distance(bsp.position, target.target.position) 
                        / gun.speed));
                    Debug.DrawLine(bsp.position, position);
                }
                else if (aiGun != null)
                {
                    position = (Vector2)target.target.position 
                        + (target.targetRB.velocity 
                        * (Vector2.Distance(bsp.position, target.target.position) 
                        / aiGun.speed));
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
        }
        if (doRotate)
        {
            Vector2 direction = (position - transform.position).normalized;
            if (limitRot)
            {
                maxRotX = limitRotValues.x + Vector2.SignedAngle(Vector2.up, parent.up);
                maxRotY = limitRotValues.y + Vector2.SignedAngle(Vector2.up, parent.up);
            }
            float unclampedAngle = Vector2.SignedAngle(Vector2.up, direction);
            float angle = Extension.ClampAngle(unclampedAngle, maxRotX, maxRotY);
            if (limitRot)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotSpeed);
                float normal = parent.eulerAngles.z;
                while (normal > 180) normal -= 360;
                while (unclampedAngle > 180) unclampedAngle -= 360;
                //Debug.Log("normal is " + normal);

                if (noFireWhenOutOfReach && (unclampedAngle - normal <= limitRotValues.x || unclampedAngle - normal >= limitRotValues.y))
                {
                    gun.active = false;
                    //Debug.Log("Kake");
                }
                else gun.active = true;
            }
                
            else transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, unclampedAngle), rotSpeed);

            //float normal = parent.eulerAngles.z;
            //while (normal > 180) normal -= 360;
            //while (unclampedAngle > 180) unclampedAngle -= 360;
            ////Debug.Log("normal is " + normal);

            //if (noFireWhenOutOfReach && (unclampedAngle - normal <= limitRotValues.x || unclampedAngle - normal >= limitRotValues.y))
            //{
            //    gun.active = false;
            //    Debug.Log("Kake");
            //}
            //else gun.active = true;
            //Debug.Log("uncl - parent " + (unclampedAngle - normal));
            //Debug.Log("transform is " + (transform.localRotation.eulerAngles.z));

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
