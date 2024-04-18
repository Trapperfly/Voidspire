using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFreeze : MonoBehaviour
{
    void OnEnable()
    {
        SetTimeScale(0);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
