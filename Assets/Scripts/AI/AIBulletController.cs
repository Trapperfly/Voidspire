using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletController : MonoBehaviour
{
    [SerializeField] float lifetime;
    [SerializeField] bool homing;
    [SerializeField] float homingStrength;
    private void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            AIBullet _aiBullet = child.GetComponent<AIBullet>();
            if (_aiBullet == null)
            {
                Debug.Log("Bullet is null, help!");
            }
            else
            {
                if (homing && _aiBullet.target != null)
                {
                    Rigidbody2D rb = _aiBullet.GetComponent<Rigidbody2D>();
                    Vector2 direction = (Vector2)_aiBullet.target.transform.position - rb.position;
                    float rotateAmount = Vector3.Cross(direction.normalized, _aiBullet.transform.up).z;
                    rb.angularVelocity = -_aiBullet.homingStrength * rotateAmount;
                    rb.velocity = _aiBullet.transform.up * _aiBullet.speed;
                }
                if (_aiBullet.currTime <= Time.time - lifetime)
                {
                    Destroy(_aiBullet.gameObject);
                }
            }
        }
    }
}
