using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class StarsParallaxCopyPos : MonoBehaviour
{
    Material mat;
    public float damping;
    public float voronoiSpeed;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mat = sr.materials[0];
        //mat.SetFloat("_Rot1", Random.Range(0, 361));
        //mat.SetFloat("_Rot2", Random.Range(0, 361));
        //mat.SetFloat("_Rot3", Random.Range(0, 361));
        mat.SetVector("_NebulaOffset", new Vector2(R(),R()));
        mat.SetVector("_NebulaOffset_1", new Vector2(R(), R()));
        mat.SetVector("_Offset1", new Vector2(R(), R()));
        mat.SetVector("_Offset2", new Vector2(R(), R()));
        mat.SetVector("_Offset3", new Vector2(R(), R()));
    }

    // Update is called once per frame
    void Update()
    {
        Transform parent = transform.parent.parent.parent;
        var rotation = Quaternion.LookRotation(parent.forward);
        transform.rotation = Quaternion.Inverse(rotation);
        mat.SetVector("_TargetPos", new Vector2(transform.position.x, transform.position.y));
        mat.SetFloat("_NebulaVoronoi", mat.GetFloat("_NebulaVoronoi") + (voronoiSpeed * Time.deltaTime));
    }
    int R()
    {
        return Random.Range(-100, 100);
    }
}
