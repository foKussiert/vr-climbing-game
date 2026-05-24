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

    private bool leftHandCanClimb;
    private bool rightHandCanClimb;

    private bool leftGripping;
    private bool rightGripping;

    private Vector3 lastLeftHandPosition;
    private Vector3 lastRightHandPosition;

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    void Start()
    {
        leftHandDevice  = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!leftHandDevice.isValid)
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (!rightHandDevice.isValid)
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool leftGrip  = false;
        bool rightGrip = false;
        leftHandDevice.TryGetFeatureValue(CommonUsages.gripButton,  out leftGrip);
        rightHandDevice.TryGetFeatureValue(CommonUsages.gripButton, out rightGrip);

        bool leftActive  = leftHandCanClimb  && leftGrip;
        bool rightActive = rightHandCanClimb && rightGrip;

        if (leftActive  && !leftGripping)  lastLeftHandPosition  = leftHand.position;
        if (rightActive && !rightGripping) lastRightHandPosition = rightHand.position;

        leftGripping  = leftActive;
        rightGripping = rightActive;

        if (leftGripping || rightGripping)
            ApplyClimbMovement();
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

        Vector3 handDelta = totalDelta / activeCount;
        Vector3 playerMove = -handDelta * climbSpeedMultiplier;

        if (characterController != null)
            characterController.Move(playerMove);
        else
            xrOrigin.position += playerMove;

        if (leftGripping)  lastLeftHandPosition  = leftHand.position;
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
