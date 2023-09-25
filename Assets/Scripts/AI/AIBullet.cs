using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBullet : MonoBehaviour
{
    public float damage;
    public float currTime;
    private void Awake()
    {
        currTime = Time.time;
    }
}
