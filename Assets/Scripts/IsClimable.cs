using UnityEngine;

public class IsClimable : MonoBehaviour
{
    public Climbing climbScript;
    public HandVisibility handVisibility;
    public enum Hand { Left, Right }
    public Hand hand;

    private void TriggerHaptic()
    {
        XRNode node = hand == Hand.Left
            ? XRNode.LeftHand
            : XRNode.RightHand;

        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (device.isValid)
        {
            device.SendHapticImpulse(0, 0.7f, 0.1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            if (hand == Hand.Left)
                climbScript.SetLeftHandCanClimb(true);
            else
                climbScript.SetRightHandCanClimb(true);

            handVisibility.SetTransparent(true);
            TriggerHaptic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            if (hand == Hand.Left)
                climbScript.SetLeftHandCanClimb(false);
            else
                climbScript.SetRightHandCanClimb(false);

            handVisibility.SetTransparent(false);
        }
    }
}
