using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public float[] audioSettings;
    public bool hudSetting;
    public GameObject[] hudItems;
    public Image[] hudCorners;
    public Sprite[] hudSprites;

    public GameObject settingsMenu;

    public VCAController[] controllers;
    public float[] defaultVolumeSetting;
    public GameObject[] hudSettingGO;
    public bool defaultHudSetting;

    public static SettingsManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.controllers = controllers;
            Instance.settingsMenu = settingsMenu;
            Instance.hudItems = hudItems;
            Instance.hudCorners = hudCorners;
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator StartSettingsMenu()
    {
        settingsMenu.SetActive(true);
        yield return new WaitForFixedUpdate();
        settingsMenu.SetActive(false);
        yield return null;
    }
    private void Start()
    {
        StartCoroutine(nameof(StartSettingsMenu));
        SetHudSetting(true);
    }

    public void SetHudSetting(bool onOff)
    {
        if (hudItems == null) return;
        foreach (var item in hudItems)
        {
            item.SetActive(onOff);
        }
        int o = 0;
        int i = 0;
        if (!onOff) o += 2;
        foreach (var item in hudCorners)
        {
            hudCorners[i].sprite = hudSprites[o];
            i++;
            o++;
        }
    }

    public void SetToDefault()
    {
        for (int i = 0; i < audioSettings.Length; i++)
        {
            audioSettings[i] = defaultVolumeSetting[i];
        }
        hudSetting = defaultHudSetting;
        SetHudSetting(!hudSetting);
        foreach (var item in controllers)
        {
            item.SetVolume(0.5f);
            item.gameObject.GetComponent<Slider>().value = 0.5f;
        }
        if (!defaultHudSetting)
        {
            hudSettingGO[0].SetActive(true);
            hudSettingGO[1].SetActive(false);
        }
        else
        {
            hudSettingGO[0].SetActive(false);
            hudSettingGO[1].SetActive(true);
        }
    }
}
