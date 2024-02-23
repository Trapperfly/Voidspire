using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] EquipmentController equipmentController;
    private void Awake()
    {
        //equipmentController.SetSingleton();
    }
}
