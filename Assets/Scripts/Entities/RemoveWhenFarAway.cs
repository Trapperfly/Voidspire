using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWhenFarAway : MonoBehaviour
{
    Transform player;
    public float maxDistance;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(nameof(DistanceCheckAndDestroy));
    }
    IEnumerator DistanceCheckAndDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Vector2.Distance(transform.position, player.position) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
