using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    Rigidbody2D debrisRb;
    float startHealth;
    [SerializeField] float currentHealth;
    GameObject player;
    [SerializeField] GameObject resourceSpawner;
    [HideInInspector] public float overkill;
    private Collider2D col;
    private Vector3 vel;
    private float angularVel;
    private Vector3 position;
    private void Awake()
    {
        SetSize();
        debrisRb = GetComponent<Rigidbody2D>();
        resourceSpawner = GameObject.Find("SpawnResourceHandler");
        player = GameObject.FindGameObjectWithTag("Player");
        float size = (transform.localScale.x + transform.localScale.y) / 2;
        debrisRb.mass = Mathf.Pow(size, 3);
        startHealth = Mathf.Pow(size + 1, 3);
        currentHealth = startHealth;
        SetDirection();
        debrisRb.AddForce(SetDirection() * Random.Range(0, debrisRb.mass / 5), ForceMode2D.Impulse);
        StartCoroutine(nameof(DistanceCheckAndDestroy));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            currentHealth -= collision.collider.GetComponent<Bullet>()._damage;
            if (currentHealth <= 0)
            {
                overkill = currentHealth;
                StartCoroutine(resourceSpawner.GetComponent<SpawnResourceOnDestroy>().SpawnResources(transform.localScale.x, overkill, transform.position));
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            currentHealth -= collision.GetComponent<Bullet>()._damage;
            if (currentHealth <= 0)
            {
                overkill = currentHealth;
                StartCoroutine(resourceSpawner.GetComponent<SpawnResourceOnDestroy>().SpawnResources(transform.localScale.x, overkill, transform.position));
                Destroy(gameObject);
            }
        }
    }
    IEnumerator DistanceCheckAndDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (Vector2.Distance(transform.position, player.transform.position) >= GetComponentInParent<SpawnDebris>().maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
    void SetSize()
    {
        float weight = Random.Range(1, 5);
        if (weight < 4)
        {
            float size = Random.Range(GetComponentInParent<SpawnDebris>().minSize, GetComponentInParent<SpawnDebris>().maxSize / 3);
            transform.localScale = new Vector2(size, size);
        }
        else if (weight == 4)
        {
            float size = Random.Range(GetComponentInParent<SpawnDebris>().minSize, GetComponentInParent<SpawnDebris>().maxSize);
            transform.localScale = new Vector2(size, size);
        }
        else
        {
            float size = Random.Range(GetComponentInParent<SpawnDebris>().minSize * 200, GetComponentInParent<SpawnDebris>().maxSize * 2);
            transform.localScale = new Vector2(size, size);
        }
    }

    Vector2 SetDirection()
    {
        float _x = Random.Range(-1, 2);
        float _y = Random.Range(-1, 2);
        return new Vector2(_x, _y);
    }
}
