using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class HitEffectController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float lifeTime;
    [Header("Fade out")]
    [SerializeField] bool fadeOut;
    [SerializeField] AnimationCurve fadeOutOverLifetimeCurve;
    [Header("Size")]
    [SerializeField] float size;
    [SerializeField] bool changeSizeOverLifetime;
    [SerializeField] AnimationCurve sizeOverLifetimeCurve;
    [SerializeField] bool randomSize;
    [SerializeField] AnimationCurve randomSizeCurve;
    float effectValue;
    Material mat;
    [Header("Colors")]
    [SerializeField] Color32 BaseColor;
    [SerializeField] Color32 EffectColor;
    [SerializeField] Color32 EmissionColor;
    float effectPercent;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (randomSize) size *= CurveRandom(randomSizeCurve);
        effectPercent = 1;
        effectValue = 60 * lifeTime;
        if (fadeOut) mat.SetFloat("_Diffuse", CurveOverTime(fadeOutOverLifetimeCurve));
        if (changeSizeOverLifetime) transform.localScale = new Vector3(size, size, size) * CurveOverTime(sizeOverLifetimeCurve);
        else transform.localScale = new Vector3(size, size, size);
        mat.SetColor("_BaseColor", BaseColor);
        mat.SetColor("_EffectColor", EffectColor);
        mat.SetColor("_EmissionColor", EmissionColor);
    }

    private void FixedUpdate()
    {
        if (fadeOut || changeSizeOverLifetime) effectPercent = effectValue / (60 * lifeTime);
        if (fadeOut) mat.SetFloat("_Diffuse", CurveOverTime(fadeOutOverLifetimeCurve));
        if (changeSizeOverLifetime) transform.localScale = new Vector3(size, size, size) * CurveOverTime(sizeOverLifetimeCurve);
        effectValue--;
        if (effectValue <= 0) Destroy(gameObject);
    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(1f - effectPercent);
    }

    float CurveRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
}
