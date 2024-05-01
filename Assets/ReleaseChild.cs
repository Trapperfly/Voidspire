using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseChild : MonoBehaviour
{
    bool quitting;
    public Transform[] child;
    public Transform toWhatParent;
    private void OnDisable()
    {
        if (MenuController.Instance.isChangingScene || quitting) return;
        foreach (Transform t in child) { t.parent = null; if (t.GetComponent<DestroyPS>()) t.GetComponent<DestroyPS>().enabled = true; }
    }
    private void OnApplicationQuit()
    {
        quitting = true;
    }
}
