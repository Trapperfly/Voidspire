using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWith : MonoBehaviour
{
    [SerializeField] Transform moveWith;
    [SerializeField] bool awake;
    [SerializeField] bool start;
    [SerializeField] bool update;

    private void Awake()
    {
        if (awake) transform.position = moveWith.position;
    }
    private void Start()
    {
        if (start) transform.position = moveWith.position;
    }
    private void Update()
    {
        if (update) transform.position = moveWith.position;
    }
}