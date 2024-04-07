using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetRotOfGP : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Transform parent = transform.parent.parent.parent;
        var rotation = Quaternion.LookRotation(parent.forward);
        transform.rotation = Quaternion.Inverse(rotation);
    }
}
