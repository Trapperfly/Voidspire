using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = player.rotation;
    }
}
