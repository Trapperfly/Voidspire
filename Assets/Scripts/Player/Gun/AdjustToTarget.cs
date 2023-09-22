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
    [SerializeField] ActiveTarget target;
    private void Awake()
    {
        target = player.GetComponent<ActiveTarget>();
    }
    private void Update()
    {
        if (target.target != null)
        {
            transform.position = target.target.position + (player.transform.position - target.target.position) / 2;
            if (Vector2.Distance(player.transform.position, target.target.position) >= maxDistance)
                player.GetComponent<TargetStandard>().RemoveTarget();
        }
        else
        {
            if ((Vector2)transform.localPosition != new Vector2(0, 0))
                transform.localPosition = new Vector2(0, 0);
            return;
        }
    }
}
