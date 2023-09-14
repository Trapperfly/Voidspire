using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimGunPointToMouse : MonoBehaviour
{
    AnimationGunController _gun;
    private void Awake()
    {
        _gun = GetComponent<AnimationGunController>();
    }
    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var direction = (mousePosition - transform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.up, direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), _gun.rotationSpeed);

        /*
        Vector3 vectorToTarget = mousePosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * _gun.rotationSpeed);
        */
    }
}
