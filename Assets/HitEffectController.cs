using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class HitEffectController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] bool fadeOut;
    [SerializeField] float size;
    [SerializeField] bool changeSizeOVerLifetime;
    [SerializeField] AnimationCurve sizeOverLifetime;
    float effectValue;
    Material mat;
    [SerializeField] Color32 BaseColor;
    [SerializeField] Color32 EffectColor;
    [SerializeField] Color32 EmissionColor;
    float effectPercent;

    private void Awake()
    {
        transform.localScale *= size;
        mat = GetComponent<SpriteRenderer>().material;
        effectValue = 60 * lifeTime;
        mat.SetColor("_BaseColor", BaseColor);
        mat.SetColor("_EffectColor", EffectColor);
        mat.SetColor("_EmissionColor", EmissionColor);
    }

    private void FixedUpdate()
    {
        if (fadeOut)
        {
            effectPercent = effectValue / (60 * lifeTime);
            mat.SetFloat("_Diffuse", effectPercent);
        }
        if (changeSizeOVerLifetime)
        {
            transform.localScale = new Vector3(size, size, 0) * CurveOverTime(sizeOverLifetime);
        }
        effectValue--;
        if (effectValue <= 0) Destroy(gameObject);
    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(1f - effectPercent);
    }
}
