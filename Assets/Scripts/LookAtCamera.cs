using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;
    private Transform cachedCameraTransform;

    private void Awake()
    {
        if (Camera.main != null)
        {
            cachedCameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (cachedCameraTransform == null)
        {
            if (Camera.main == null)
            {
                return;
            }
            cachedCameraTransform = Camera.main.transform;
        }

        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(cachedCameraTransform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - cachedCameraTransform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = cachedCameraTransform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -cachedCameraTransform.forward;
                break;
        }
    }
}
