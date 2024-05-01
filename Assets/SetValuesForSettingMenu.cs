using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValuesForSettingMenu : MonoBehaviour
{
    public GameObject b1;
    public GameObject b2;

    private void Start()
    {
        StartCoroutine(nameof(WaitAFrame));
    }

    IEnumerator WaitAFrame()
    {
        yield return new WaitForFixedUpdate();
        SettingsManager.Instance.SetHudSetting(!SettingsManager.Instance.hudSetting);
        if (!SettingsManager.Instance.hudSetting)
        {
            b1.SetActive(true);
            b2.SetActive(false);
        }
        else
        {
            b1.SetActive(false);
            b2.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
