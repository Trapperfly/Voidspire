using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float followSpeed;
    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }
        transform.position = Vector2.Lerp(transform.position, (Vector2)target.position + offset, followSpeed);
    }
}
