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

    private float verticalVelocity = 0f;

    private float leftBuffer;
    private float rightBuffer;

    public float bufferTime = 0.1f;

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

        bool leftValid = leftHandCanClimb && leftGrip;
        bool rightValid = rightHandCanClimb && rightGrip;

        if (leftValid)
            leftBuffer = bufferTime;
        else
            leftBuffer -= Time.deltaTime;

        if (rightValid)
            rightBuffer = bufferTime;
        else
            rightBuffer -= Time.deltaTime;

        bool leftActive = leftBuffer > 0;
        bool rightActive = rightBuffer > 0;

        if (leftActive  && !leftGripping)  lastLeftHandPosition  = leftHand.position;
        if (rightActive && !rightGripping) lastRightHandPosition = rightHand.position;

        leftGripping  = leftActive;
        rightGripping = rightActive;

        if (leftActive || rightActive)
        {
            verticalVelocity = 0f;
            ApplyClimbMovement();
        }
        else if (characterController != null)
        {
            if (characterController.isGrounded)
                verticalVelocity = 0f;
            else
                verticalVelocity += Physics.gravity.y * Time.deltaTime;

            Vector3 move = Vector3.zero;

            if (leftActive || rightActive)
            {
                verticalVelocity = 0f;
                ApplyClimbMovement();
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
                move.y = verticalVelocity * Time.deltaTime;
            }

            characterController.Move(move);
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
