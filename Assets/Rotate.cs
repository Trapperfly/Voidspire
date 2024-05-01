using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    float i = 0;
    void Update()
    {
        i += speed * Time.deltaTime;
        transform.eulerAngles = new(0, 0, i);
    }
}
