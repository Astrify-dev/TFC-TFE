using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ParallaxLayer : MonoBehaviour
{

    [Tooltip("Référence à la caméra principale (optionnel, auto-assigné si null)")]
    public Camera mainCamera;

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            previousCameraPosition = mainCamera.transform.position;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        Vector3 cameraDelta = mainCamera.transform.position - previousCameraPosition;

        float depthX = Mathf.Abs(transform.position.x - mainCamera.transform.position.x);
        float effectiveParallax = Mathf.Clamp01(1f / (1f + depthX)); // mapping simple

        transform.position -= new Vector3(cameraDelta.x * effectiveParallax, cameraDelta.y * effectiveParallax, 0f);

        previousCameraPosition = mainCamera.transform.position;
    }
}