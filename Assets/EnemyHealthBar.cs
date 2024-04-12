using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public bool isBoss;
    public Transform target;
    public Vector2 offset;
    public float followSpeed;
    public float visibilityTime;
    Rigidbody2D rb;
    public float timer;

    private void Start()
    {
        rb = target.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        
        if (target == null) { Destroy(gameObject); return; }
        if (!target.gameObject.activeSelf) { return; }//InvokeRepeating(nameof(CheckIfSimulated),0,1); Debug.LogError("stuff"); return; }
        transform.position = Vector2.Lerp(transform.position, (Vector2)target.position + offset, followSpeed);
        timer += Time.deltaTime;
        if (isBoss) { return; }
        if (timer > visibilityTime) Destroy(gameObject);
    }
    //void CheckIfSimulated()
    //{
    //    if (rb.simulated) gameObject.SetActive(true);
    //    CancelInvoke(nameof(CheckIfSimulated));
    //}
    private void OnDestroy()
    {
        //CancelInvoke(nameof(CheckIfSimulated));
    }
}
