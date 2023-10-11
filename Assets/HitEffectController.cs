using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class HitEffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    float currentDiffuse;
    Material mat;
    [SerializeField] Color32 BaseColor;
    [SerializeField] Color32 EffectColor;
    [SerializeField] Color32 EmissionColor;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        currentDiffuse = 60 * lifeTime;
        mat.SetColor("_BaseColor", BaseColor);
        mat.SetColor("_EffectColor", EffectColor);
        mat.SetColor("_EmissionColor", EmissionColor);
    }

    private void FixedUpdate()
    {
        float diffusePercent = currentDiffuse / (60 * lifeTime);
        mat.SetFloat("_Diffuse", diffusePercent);
        currentDiffuse--;
        if (currentDiffuse <= 0) Destroy(gameObject);
    }
}
