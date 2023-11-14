using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFade : MonoBehaviour
{
    [SerializeField] AnimationCurve fadeCurve;
    [SerializeField] float duration;
    [SerializeField] Image targetImage;
    [SerializeField] Color color = new(1,1,1);
    float currTime;
    float timePercent;

    private void Awake()
    {
        timePercent = currTime / duration;
        targetImage.color = new Color(color.r, color.g, color.b, CurveOverTime(fadeCurve));
    }
    void Update()
    {
        timePercent = currTime / duration;
        targetImage.color = new Color(color.r, color.g, color.b, CurveOverTime(fadeCurve));
        currTime += 1 * Time.deltaTime;
    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(1-timePercent);
    }
}
