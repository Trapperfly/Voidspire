using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToTarget : MonoBehaviour
{
    public Camera cam;
    [SerializeField] bool zoom;
    [SerializeField] float zoomValue;
    //float baseZoomValue = 5f;
    //[SerializeField] float maxDistance = 10f;
    [SerializeField] GameObject player;
    [SerializeField] ActiveTarget target;
    public Transform holder;
    [Range(0,10)]
    public float speed;
    private void Awake()
    {
        cam = Camera.main;
        target = player.GetComponent<ActiveTarget>();
    }
    private void LateUpdate()
    {
        if (target.target)
        {   
            if (transform.parent != holder) { transform.SetParent(holder); }
            transform.position 
                = Vector2.Lerp(transform.position, target.target.position + (player.transform.position - target.target.position) / 2, speed * Time.deltaTime);
            if (cam.WorldToViewportPoint(target.target.position).x > 1f
                || cam.WorldToViewportPoint(target.target.position).y > 1f 
                || cam.WorldToViewportPoint(target.target.position).x < 0f
                || cam.WorldToViewportPoint(target.target.position).y < 0f)
                player.GetComponent<TargetStandard>().RemoveTarget();
            //if (Vector2.Distance(player.transform.position, target.target.position) >= maxDistance)
            //    player.GetComponent<TargetStandard>().RemoveTarget();
        }
        else
        {
            if (transform.parent != player.transform) { transform.SetParent(player.transform); }
            transform.position = Vector2.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
            return;
        }
    }
}
