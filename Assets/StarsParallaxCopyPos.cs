using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class StarsParallaxCopyPos : MonoBehaviour
{
    Material mat;
    public float damping;
    public float voronoiSpeed;
    public bool mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out Image image);
        TryGetComponent(out SpriteRenderer spriteRenderer);
        if (image) mat = image.material;
        if (spriteRenderer) mat = spriteRenderer.materials[0];
        //mat.SetFloat("_Rot1", Random.Range(0, 361));
        //mat.SetFloat("_Rot2", Random.Range(0, 361));
        //mat.SetFloat("_Rot3", Random.Range(0, 361));
        mat.SetVector("_NebulaOffset", new Vector2(R(),R()));
        mat.SetVector("_NebulaOffset_1", new Vector2(R(), R()));
        mat.SetVector("_Offset1", new Vector2(R(), R()));
        mat.SetVector("_Offset2", new Vector2(R(), R()));
        mat.SetVector("_Offset3", new Vector2(R(), R()));
        if (mainMenu)
        {
            mat.SetVector("_NebulaOffset", new Vector2(2000, 2000));
            mat.SetVector("_NebulaOffset_1", new Vector2(1000, 1000));
            mat.SetVector("_Offset1", new Vector2(200, 200));
            mat.SetVector("_Offset2", new Vector2(500, 500));
            mat.SetVector("_Offset3", new Vector2(1000, 1000));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMenu)
        {
            mat.SetVector("_Offset1",
                Vector2.Lerp(mat.GetVector("_Offset1"),
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) / 100 + new Vector2(200, 200),
                damping));
            mat.SetVector("_Offset2",
                Vector2.Lerp(mat.GetVector("_Offset2"),
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) / 60 + new Vector2(500, 500),
                damping));
            mat.SetVector("_Offset3",
                Vector2.Lerp(mat.GetVector("_Offset3"),
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) / 30 + new Vector2(1000, 1000),
                damping));
            mat.SetVector("_NebulaOffset", 
                Vector2.Lerp(mat.GetVector("_NebulaOffset"),
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) / 2000 + new Vector2(2000, 2000), 
                damping));
            mat.SetVector("_NebulaOffset_1",
                Vector2.Lerp(mat.GetVector("_NebulaOffset_1"),
                new Vector2(Input.mousePosition.x, Input.mousePosition.y) / 500 + new Vector2(1000, 1000),
                damping));
            return;
        }
        Transform parent = transform.parent.parent.parent;
        var rotation = Quaternion.LookRotation(parent.forward);
        transform.rotation = Quaternion.Inverse(rotation);
        mat.SetVector("_TargetPos", new Vector2(transform.position.x, transform.position.y));
        //mat.SetFloat("_NebulaVoronoi", mat.GetFloat("_NebulaVoronoi") + (voronoiSpeed * Time.deltaTime));
    }
    int R()
    {
        return Random.Range(-100, 100);
    }
}
