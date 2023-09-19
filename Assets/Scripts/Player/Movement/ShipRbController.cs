using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRbController : MonoBehaviour
{
    [SerializeField] bool resetOnAwake = true;
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (resetOnAwake)   //Resets center if askew
        {
            Vector2 newCenter = new(0, 0);
            rb.centerOfMass = newCenter;
        }
    }
}
