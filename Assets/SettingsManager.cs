using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public float[] audioSettings;
    public bool hudSetting;

    public GameObject settingsMenu;

    public static SettingsManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
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
    }
}
