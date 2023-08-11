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

    private void Awake()
    {
        currTime = Time.time;
    }
    void Update()
    {
        if (currTime <= Time.time - _bulletLongevity)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            //if (collision.GetComponent<Rigidbody2D>() != null)
                //collision.GetComponent<Rigidbody2D>().AddForce()
            Destroy(gameObject);
        }
    }
}