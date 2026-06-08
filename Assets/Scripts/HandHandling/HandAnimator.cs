using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    [System.Serializable]
    public class Finger
    {
        public Transform root;
        public Transform mid;
        public Transform tip;
        public Quaternion openRoot,   openMid,   openTip;
        public Quaternion closedRoot, closedMid, closedTip;
    }

    [Header("Fingers")]
    public Finger thumb;
    public Finger indexFinger;
    public Finger middleFinger;
    public Finger ringFinger;
    public Finger littleFinger;

    [Header("Settings")]
    public float gripSpeed = 10f;

    private Finger[] fingers;
    private float gripAmount;

    void InitFingers()
    {
        fingers = new[] { thumb, indexFinger, middleFinger, ringFinger, littleFinger };
    }

    protected void Start() => InitFingers();

    [ContextMenu("Save Open Pose")]
    void SaveOpenPose()
    {
        InitFingers();
        foreach (var f in fingers)
        {
            f.openRoot = f.root.localRotation;
            f.openMid  = f.mid.localRotation;
            f.openTip  = f.tip.localRotation;
        }
    }

    [ContextMenu("Save Closed Pose")]
    void SaveClosedPose()
    {
        InitFingers();
        foreach (var f in fingers)
        {
            f.closedRoot = f.root.localRotation;
            f.closedMid  = f.mid.localRotation;
            f.closedTip  = f.tip.localRotation;
        }
    }

    void Update()
    {

        // bool gripping = IsGripping();
        // Debug.Log($"IsGripping: {gripping}, gripAmount: {gripAmount}");

        gripAmount = Mathf.Lerp(gripAmount, IsGripping() ? 1f : 0f, Time.deltaTime * gripSpeed);

        foreach (var f in fingers)
        {
            f.root.localRotation = Quaternion.Lerp(f.openRoot,   f.closedRoot,   gripAmount);
            f.mid.localRotation  = Quaternion.Lerp(f.openMid,    f.closedMid,    gripAmount);
            f.tip.localRotation  = Quaternion.Lerp(f.openTip,    f.closedTip,    gripAmount);
        }
    }

    protected virtual bool IsGripping() => false;

    public void PreviewOpen()
    {
        InitFingers();
        foreach (var f in fingers)
        {
            f.root.localRotation = f.openRoot;
            f.mid.localRotation  = f.openMid;
            f.tip.localRotation  = f.openTip;
        }
    }

    public void PreviewClosed()
    {
        InitFingers();
        foreach (var f in fingers)
        {
            f.root.localRotation = f.closedRoot;
            f.mid.localRotation  = f.closedMid;
            f.tip.localRotation  = f.closedTip;
        }
    }
}
