using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetListener : MonoBehaviour
{
    public bool setSimpleHud;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { SettingsManager.Instance.SetHudSetting(setSimpleHud); });
    }
}
