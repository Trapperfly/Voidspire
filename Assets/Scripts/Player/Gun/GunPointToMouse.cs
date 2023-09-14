using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class GunPointToMouse : MonoBehaviour
{
    GunController _gun;
    Transform player;
    float maxRotX;
    float maxRotY;
    private void Awake()
    {
        _gun = GetComponent<GunController>();
        player = transform.parent;
    }
    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (mousePosition - transform.position).normalized;

        maxRotX = _gun.rotationAngle.x + Vector2.SignedAngle(Vector2.up, player.up);
        maxRotY = _gun.rotationAngle.y + Vector2.SignedAngle(Vector2.up, player.up);

        float unclampedAngle = Vector2.SignedAngle(Vector2.up, direction);
        float angle = Extension.ClampAngle(unclampedAngle, maxRotX, maxRotY);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), _gun.rotationSpeed);
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
