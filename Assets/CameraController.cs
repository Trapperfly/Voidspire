using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public float zoom = 5;
    public float minZoom = 3;
    public float maxZoom = 10;

    public TMP_Text text;
    CinemachineVirtualCamera virtualCamera;
    LensSettings settings;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        settings = virtualCamera.m_Lens;
    }

    private void Update()
    {
        zoom -= Input.mouseScrollDelta.y;
        if (zoom < minZoom) { zoom = minZoom; } else if (zoom > maxZoom) { zoom = maxZoom; }
        settings.OrthographicSize = Mathf.Lerp(settings.OrthographicSize, zoom, 0.03f);
        virtualCamera.m_Lens = settings;
        text.text = Mathf.RoundToInt(settings.OrthographicSize).ToString("F0");
    }
}
