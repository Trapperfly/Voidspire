using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWhenFarAway : MonoBehaviour
{
    Vector3 playerPos;
    public float maxDistance;
    void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        StartCoroutine(nameof(DistanceCheckAndDestroy));
    }
    IEnumerator DistanceCheckAndDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Vector2.Distance(transform.position, playerPos) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
