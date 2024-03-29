using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class StarsParallaxCopyPos : MonoBehaviour
{
    Material mat;
    public float damping;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mat = sr.materials[0];
        mat.SetFloat("_Rot1", Random.Range(0, 361));
        mat.SetFloat("_Rot2", Random.Range(0, 361));
        mat.SetFloat("_Rot3", Random.Range(0, 361));
    }

    // Update is called once per frame
    void Update()
    {
        Transform parent = transform.parent.parent.parent;
        var rotation = Quaternion.LookRotation(parent.forward);
        transform.rotation = Quaternion.Inverse(rotation);
        mat.SetVector("_TargetPos", new Vector2(transform.position.x, transform.position.y));
    }
}
