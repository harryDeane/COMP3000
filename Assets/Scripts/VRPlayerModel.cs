using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class VRPlayerModel : MonoBehaviour
{
    public XROrigin xrOrigin; // XR Rig reference
    public Transform headModel; // Model's head
    public Transform bodyModel; // Model's body
    public Transform headset; // VR headset transform

    private Vector3 initialOffset;

    void Start()
    {
        if (xrOrigin == null) xrOrigin = FindObjectOfType<XROrigin>();

        // Get the VR headset reference from the XR Rig
        headset = xrOrigin.Camera.transform;
        initialOffset = transform.position - headset.position;
    }

    void Update()
    {
        if (headset == null) return;

        // Make model follow the player's position
        transform.position = headset.position + initialOffset;

        // Rotate the model's head to match the headset rotation
        headModel.rotation = headset.rotation;

        // Keep the body rotation aligned with horizontal head rotation
        Vector3 bodyEulerAngles = bodyModel.eulerAngles;
        bodyEulerAngles.y = headset.eulerAngles.y; // Only rotate on Y-axis
        bodyModel.eulerAngles = bodyEulerAngles;
    }
}
