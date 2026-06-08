using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandAnimator), true)]
public class HandAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandAnimator hand = (HandAnimator)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Preview Open Pose"))
            hand.PreviewOpen();

        if (GUILayout.Button("Preview Closed Pose"))
            hand.PreviewClosed();
    }
}
