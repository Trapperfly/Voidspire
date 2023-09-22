using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public float damageChange;
    public float sizeChange;
    public float speed;
    public float speedChange;
    public float bulletLongevity;
    public int pierce;
    public int bounce;
    public bool bounceToTarget;
    public bool homing;
    public float homingStrength;
    public float punch;
    public bool trigger;
    public float weightScalar;
    public Transform target;
    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            Bullet _bullet = child.GetComponent<Bullet>();
            if (_bullet == null)
            {
                Debug.Log("Bullet is null, Help!");
            }
            else
            {
                if (_bullet._localHoming && target != null)
                {
                    Vector2 direction = (Vector2)target.transform.position - _bullet.rb.position;
                    float rotateAmount = Vector3.Cross(direction.normalized, _bullet.transform.up).z;
                    _bullet.rb.angularVelocity = -homingStrength * rotateAmount;
                    _bullet.rb.velocity = _bullet.transform.up * speed;
                }
                else
                {
                    _bullet.transform.up = _bullet.rb.velocity;
                    _bullet.rb.angularVelocity = 0;
                }
                if (_bullet.currTime <= Time.time - bulletLongevity)
                {
                    Destroy(_bullet.gameObject);
                }
            }
        }
    }
}
