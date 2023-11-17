using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFade : MonoBehaviour
{
    [SerializeField] bool awake;
    [SerializeField] bool enable;
    [SerializeField] int times = 1;
    [SerializeField] AnimationCurve fadeCurve;
    [SerializeField] float duration;
    [SerializeField] Image[] targetImages;
    [SerializeField] Color color = new(1,1,1);
    float currTime;
    float timePercent;

    bool active;
    bool back;
    int localTimes;

    private void Awake()
    {
        timePercent = currTime / duration;
        foreach (var image in targetImages)
        {
            image.color = new Color(color.r, color.g, color.b, CurveOverTime(fadeCurve));
        }

        if (awake) active = true;
    }
    void Update()
    {
        if (back && active) Fade(-1);
        else if (active) Fade(1);
    }

    public void StartFade()
    {
        if (!active) localTimes = times;
        active = true;
    }

    void Fade(int timeDir)
    {
        foreach (var image in targetImages)
        {
            image.color = new Color(color.r, color.g, color.b, CurveOverTime(fadeCurve));
        }
        currTime += timeDir * Time.deltaTime;
        timePercent = currTime / duration;
        if (timePercent >= 1)
        {
            Debug.Log("percent is at " + timePercent + " so i stop");
            localTimes--;
            if (localTimes <= 0)
            {
                active = false;
            }
            
            back = true;
        }
        else if (timePercent <= 0)
        {
            Debug.Log("percent is at " + timePercent + " so i stop");
            localTimes--;
            if (localTimes <= 0)
            {
                active = false;
            }
            back = false;
        }
    }

    float CurveOverTime(AnimationCurve curve)
    {
        return curve.Evaluate(timePercent);
    }
}
