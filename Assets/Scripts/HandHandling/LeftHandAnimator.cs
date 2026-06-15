using UnityEngine.XR;

public class LeftHandAnimator : HandAnimator
{
    private InputDevice device;

    protected override void Start()
    {
        base.Start();
        device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    protected override bool IsGripping()
    {
        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        device.TryGetFeatureValue(CommonUsages.gripButton, out bool grip);
        return grip;
    }
}
