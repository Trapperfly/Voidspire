using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCAController : MonoBehaviour
{
    private FMOD.Studio.VCA vcaController;
    public string vcaName;
    public int settingRef;
    public TMPro.TMP_Text text;

    private Slider slider;
    void Start()
    {
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        vcaController.setVolume(GlobalRefs.Instance.audioSettings[settingRef]);
        text.text = (GlobalRefs.Instance.audioSettings[settingRef] * 2 * 100).ToString("F0") + "%";
    }

    public void SetVolume(float volume)
    {
        vcaController.setVolume(volume * 2);
        GlobalRefs.Instance.audioSettings[settingRef] = volume * 2;
        text.text = (volume * 2 * 100).ToString("F0") + "%";
    }
}
