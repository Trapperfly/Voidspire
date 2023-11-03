using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class HitEffectController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float lifeTime;
    [Header("Effect")]
    [SerializeField] float effectSpeed;
    [SerializeField] bool changeEffectSpeedOverLifetime;
    [SerializeField] AnimationCurve effectSpeedCurve;
    [SerializeField] float effectStrength;
    [SerializeField] bool changeEffectStrengthOverLifetime;
    [SerializeField] AnimationCurve effectStrengthCurve;
    [Header("Fade out")]
    [SerializeField] bool fadeOut;
    [SerializeField] AnimationCurve fadeOutOverLifetimeCurve;
    [Header("Size")]
    [SerializeField] float size;
    [SerializeField] bool changeSizeOverLifetime;
    [SerializeField] AnimationCurve sizeOverLifetimeCurve;
    [SerializeField] bool randomSize;
    [SerializeField] AnimationCurve randomSizeCurve;
    [Header("Noise")]
    [SerializeField] bool addNoise;
    [SerializeField] float noiseStrength;
    [SerializeField] float noiseDetail;

    [Header("Colors")]
    [SerializeField] Color32 BackColor;
    [SerializeField] Color32 BaseColor;
    [SerializeField] Color32 MidColor;
    [SerializeField] Color32 TipColor;
    [SerializeField] float tipSize;
    float effectValue;
    Material mat;
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

        if (changeEffectSpeedOverLifetime) mat.SetFloat("_EffectSpeed", effectSpeed * CurveOverTime(effectSpeedCurve));
        else mat.SetFloat("_EffectSpeed", effectSpeed);
        if (changeEffectStrengthOverLifetime) mat.SetFloat("_EffectStrength", effectStrength * CurveOverTime(effectStrengthCurve));
        else mat.SetFloat("_EffectStrength", effectStrength);
        mat.SetFloat("_EffectStartOffset", Random.Range(10, 100));
        mat.SetColor("_BackColor", BackColor);
        mat.SetColor("_BaseColor", BaseColor);
        mat.SetColor("_MidColor", MidColor);
        mat.SetColor("_TipColor", TipColor);
        mat.SetFloat("_TipSize", tipSize);
        mat.SetFloat("_EffectStartOffset", Random.value * 100);

        if (addNoise) mat.SetFloat("_NoisedDiffuse", 1);
        mat.SetFloat("_NoiseStrength", noiseStrength);
        mat.SetFloat("_NoiseDetail", noiseDetail);
    }

    private void FixedUpdate()
    {
        effectPercent = effectValue / (60 * lifeTime);
        if (fadeOut) mat.SetFloat("_Diffuse", CurveOverTime(fadeOutOverLifetimeCurve));
        if (changeSizeOverLifetime) transform.localScale = new Vector3(size, size, size) * CurveOverTime(sizeOverLifetimeCurve);
        if (changeEffectSpeedOverLifetime) mat.SetFloat("_EffectSpeed", effectSpeed * CurveOverTime(effectSpeedCurve));
        if (changeEffectStrengthOverLifetime) mat.SetFloat("_EffectStrength", effectStrength * CurveOverTime(effectStrengthCurve));

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
