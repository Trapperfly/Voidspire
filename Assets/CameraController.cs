using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float zoom = 5;
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
        if (zoom < 3) { zoom = 3; } else if (zoom > 10) { zoom = 10; }
        settings.OrthographicSize = Mathf.Lerp(settings.OrthographicSize, zoom, 0.03f);
        virtualCamera.m_Lens = settings;
    }
}
