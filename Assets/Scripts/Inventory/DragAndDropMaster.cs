using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DragAndDropMaster : MonoBehaviour
{
    public GameObject savedInfoBox;
    public Transform slot;
    #region Singleton
    public static DragAndDropMaster Instance;

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
    }
    #endregion
}
