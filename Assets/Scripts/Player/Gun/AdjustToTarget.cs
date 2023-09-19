using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToTarget : MonoBehaviour
{
    [SerializeField] bool zoom;
    [SerializeField] float zoomValue;
    //float baseZoomValue = 5f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] GameObject player;
    public GameObject target;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (target == null)
        {
            if ((Vector2)transform.localPosition != new Vector2(0, 0))
                transform.localPosition = new Vector2(0, 0);
            return;
        }
        else
        {
            transform.position = target.transform.position + (player.transform.position - target.transform.position) / 2;
            if (Vector2.Distance(player.transform.position, target.transform.position) >= maxDistance)
                player.GetComponent<TargetStandard>().RemoveTarget();
        }
    }
}
