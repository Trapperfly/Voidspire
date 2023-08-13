using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _damageChange;
    public float _sizeChange;
    public float _bulletLongevity;
    public int _pierce;
    public int _bounce;
    public bool _bounceToTarget;
    public bool _homing;
    public float _homingStrength;
    public float _punch;
    float currTime;
    Rigidbody2D rb;
    public GameObject target;
    float startVelocity;

    private void Awake()
    {
        currTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startVelocity = rb.velocity.magnitude;
    }
    void Update()
    {
        if (currTime <= Time.time - _bulletLongevity)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_homing && target != null)
        {
            /*
            Vector2 direction = (target.transform.position - transform.position)- transform.up;
            rb.AddForce(direction.normalized * _homingStrength, ForceMode2D.Force);
            //*(target.transform.position - transform.position).magnitude
            transform.up = rb.velocity;
            */
        }
        else
            transform.up = rb.velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.GetComponent<Rigidbody2D>() != null)
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * _punch);
            else if (collision.GetComponentInChildren<Rigidbody2D>() != null)
                collision.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * _punch, ForceMode2D.Impulse);
            if (_pierce > 0)
                _pierce--;
            else
                Destroy(gameObject);
        }
    }
}