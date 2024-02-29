using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletController bc;
    public GameObject bulletSender;
    public float _localDamage;
    public float _splashDamage;
    public float _splashRange;
    public int _localBounce;
    public int _localPierce;
    public bool _localHoming;
    public bool _explosive;
    public int _cluster;
    public bool _isClusterProjectile;
    public int _clusterAmount;
    public float _clusterSpeed;
    public GameObject _clusterPrefab;
    public float currTime;
    public Rigidbody2D rb;
    Collider2D col;

    bool stopping;

    public Vector3 lastVelocity;
    public bool bounced = false;

    private void Awake()
    {
        currTime = Time.time;
        bc = GetComponentInParent<BulletController>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        _localBounce = bc.bounce;
        _localPierce = bc.pierce;
        _localHoming = bc.homing;
        if (!_isClusterProjectile)
            _localDamage = bc.damage;
        _splashDamage = bc.splashDamage;
        _splashRange = bc.splashRange;
        _explosive = bc.isExplosive;
        if (!_isClusterProjectile)
            _cluster = bc.cluster;
        _clusterAmount = bc.clusterAmount;
        _clusterSpeed = bc.clusterSpeed;
    }
    private void Start()
    {
        if (!_explosive && bc.bounce > 0)
        {
            col.isTrigger = false;
        }
    }
    private void FixedUpdate()
    {
        if (bounced)
        {
            _localBounce--;
            col.isTrigger = false;
            bounced = false;
        }
        lastVelocity = rb.velocity;     //Logs last velocity to be used in bounce
    }
    public IEnumerator WaitAndSwitchHoming(float time)
    {
        _localHoming = false;
        yield return new WaitForSeconds(time * bc.homingStrength / 25);
        _localHoming = true;
        yield return new WaitForFixedUpdate();
    }
    private void OnDestroy()
    {
        if (stopping) return;
        if (_explosive)
        {
            Debug.Log(_splashDamage);
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _splashRange);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<Damagable>(out var dm)) dm.TakeDamage(_splashDamage, col.transform.position, bulletSender);
            }
        }
        if (_cluster > 0)
        {
            Debug.Log("Clustering");
            for (int i = 0; i < _clusterAmount; i++)
            {
                Rigidbody2D bullet = Instantiate(_clusterPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), transform.parent).GetComponent<Rigidbody2D>();
                Bullet b = bullet.GetComponent<Bullet>();
                b._localDamage *= 0.2f;
                b._cluster = _cluster - 1;
                b.bulletSender = bulletSender;
                b._clusterPrefab = _clusterPrefab;
                bullet.velocity = bullet.transform.up * _clusterSpeed;
            }
        }
        StopAllCoroutines();
    }
    private void OnApplicationQuit()
    {
        stopping = true;
    }
}