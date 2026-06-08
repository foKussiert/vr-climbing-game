using UnityEngine;
using UnityEngine.XR;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin;
    public Transform leftHand;
    public Transform rightHand;
    public CharacterController characterController;

    [Header("Settings")]
    public float climbSpeedMultiplier = 1.0f;
    public float bufferTime = 0.1f;

    private bool leftHandCanClimb;
    private bool rightHandCanClimb;

    private bool leftGripping;
    private bool rightGripping;

    private Vector3 lastLeftHandPosition;
    private Vector3 lastRightHandPosition;

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    private float verticalVelocity;

    private float leftBuffer;
    private float rightBuffer;

    void Start()
    {
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!leftHandDevice.isValid)
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!rightHandDevice.isValid)
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        leftHandDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
        rightHandDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);

        leftBuffer = (leftHandCanClimb && leftGrip) ? bufferTime : leftBuffer - Time.deltaTime;
        rightBuffer = (rightHandCanClimb && rightGrip) ? bufferTime : rightBuffer - Time.deltaTime;

        bool leftActive = leftBuffer > 0f;
        bool rightActive = rightBuffer > 0f;

        if (leftActive && !leftGripping)
            lastLeftHandPosition = leftHand.position;
        if (rightActive && !rightGripping)
            lastRightHandPosition = rightHand.position;

        leftGripping = leftActive;
        rightGripping = rightActive;

        if (leftActive || rightActive)
        {
            verticalVelocity = 0f;
            ApplyClimbMovement();
        }
        else
        {
            verticalVelocity = characterController.isGrounded ? 0f : verticalVelocity + Physics.gravity.y * Time.deltaTime;
            characterController.Move(new Vector3(0f, verticalVelocity * Time.deltaTime, 0f));
        }
    }

    void ApplyClimbMovement()
    {
        Vector3 totalDelta = Vector3.zero;
        int activeCount = 0;

        if (leftGripping)
        {
            totalDelta += leftHand.position - lastLeftHandPosition;
            activeCount++;
        }
        if (rightGripping)
        {
            totalDelta += rightHand.position - lastRightHandPosition;
            activeCount++;
        }

        characterController.Move(-(totalDelta / activeCount) * climbSpeedMultiplier);

        if (leftGripping) lastLeftHandPosition = leftHand.position;
        if (rightGripping) lastRightHandPosition = rightHand.position;
    }

    public void SetLeftHandCanClimb(bool value)
    {
        leftHandCanClimb = value;
        if (!value) leftGripping = false;
    }

    public void SetRightHandCanClimb(bool value)
    {
        rightHandCanClimb = value;
        if (!value) rightGripping = false;
    }
}
