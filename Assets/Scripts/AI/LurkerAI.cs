using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkerAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] float boostSpeed;
    [SerializeField] float detectionRange;
    public List<Vector2> dirs = new List<Vector2>();

    Vector2 dir;
    [SerializeField] Vector2 targetPos;
    [SerializeField] LayerMask targetMask;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;
    [SerializeField] Transform player;

    public float _distance;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        StartCoroutine(nameof(CheckProximity));
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPos = player.position;
    }
    private void FixedUpdate()
    {
        Vector2 actualDir = targetPos - rb.position;
        float rotateAmount = Vector3.Cross(actualDir.normalized, transform.up).z;
        rb.angularVelocity = -rotSpeed * (detectionRange + 1f - _distance) * rotateAmount;
        rb.velocity = speed * transform.up;
    }
    private void Update()
    {
        foreach  (Vector2 line in dirs)
        {
            Debug.DrawRay(transform.position, line.normalized);
        }
    }
    IEnumerator CheckProximity()
    {
        while (true)
        {
            dirs.Clear();
            new WaitForSeconds(0.5f);
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, detectionRange, targetMask);
            if (objects != null)
            {
                foreach (Collider2D avoid in objects)
                {
                    if (avoid != col)
                    {
                        _distance = Vector2.Distance(transform.position, avoid.ClosestPoint(transform.position));
                        dirs.Add(-((avoid.ClosestPoint(transform.position) - (Vector2)transform.position) * (detectionRange + 1f - _distance)));
                        Debug.DrawLine(transform.position, avoid.ClosestPoint(transform.position));
                    }
                }
            }
            Vector2 newDirection = new Vector2(0,0);
            if (dirs.Count > 0)
                foreach (Vector2 affectingVector in dirs)
                {
                    newDirection += affectingVector;
                }
            dir = newDirection;
            if (dirs.Count > 0)
                targetPos = (Vector2)transform.position + (dir * 10);
            else targetPos = player.position;
            Debug.DrawRay(transform.position, dir);
            yield return null;
        }
    }

    IEnumerator RandomNewTarget()
    {
        while(true)
        {
            new WaitForSeconds(5f);
            yield return null;
        }
    }
}
