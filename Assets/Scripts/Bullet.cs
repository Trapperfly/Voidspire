using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _damage;
    public float _damageChange;
    public float _sizeChange;
    public float _speed;
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
    Vector3 lastVelocity;
    bool bounced = false;
    public float _weightScalar;
    public float magnitude;
    private void Awake()
    {
        currTime = Time.time;
        rb = GetComponent<Rigidbody2D>();

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
            Vector2 direction = (Vector2)target.transform.position - rb.position;
            float rotateAmount = Vector3.Cross(direction.normalized, transform.up).z;
            rb.angularVelocity = -_homingStrength * rotateAmount;
            rb.velocity = transform.up * _speed;
            /*
            Vector2 direction = (target.transform.position - transform.position)- transform.up;
            rb.AddForce(direction.normalized * _homingStrength, ForceMode2D.Force);
            //*(target.transform.position - transform.position).magnitude
            transform.up = rb.velocity;
            */
        }
        else
        {
            transform.up = rb.velocity;
            rb.angularVelocity = 0;
        }
        lastVelocity = rb.velocity;     //Logs last velocity to be used in bounce
        if (bounced)
        {
            GetComponent<Collider2D>().isTrigger = false;
            bounced = false;
        }
        magnitude = rb.velocity.magnitude;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().isTrigger = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.collider.GetComponent<Rigidbody2D>() != null)
                collision.collider.GetComponent<Rigidbody2D>().AddForce(transform.up * _punch);
            else if (collision.collider.GetComponentInChildren<Rigidbody2D>() != null)
                collision.collider.GetComponentInChildren<Rigidbody2D>().AddForce(transform.up * _punch, ForceMode2D.Impulse);
            if (_pierce > 0)
            {
                rb.velocity = lastVelocity;
                _pierce--;
            }
            else if (_bounce > 0)
            {
                foreach (var contact in collision.contacts)
                {
                    Vector2 _bounceDir = Vector2.Reflect(lastVelocity, contact.normal).normalized;
                    rb.velocity = _bounceDir * _speed;
                    bounced = true;
                    if (_homing && target != null)
                        StartCoroutine(WaitAndSwitchHoming(0.01f));
                }
                rb.angularVelocity = 0;
                _bounce--;
            }
            else
                Destroy(gameObject);
        }
    }
    
    IEnumerator WaitAndSwitchHoming(float time)
    {
        _homing = false;
        yield return new WaitForSeconds(time * _homingStrength / 25);
        _homing = true;
        yield return new WaitForFixedUpdate();
    }
}