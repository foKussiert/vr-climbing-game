using UnityEngine.XR;

public class RightHandAnimator : HandAnimator
{
    private InputDevice device;

    protected override void Start()
    {
        base.Start();
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    protected override bool IsGripping()
    {
        if (!device.isValid)
            device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        device.TryGetFeatureValue(CommonUsages.gripButton, out bool grip);
        return grip;
    }
}
