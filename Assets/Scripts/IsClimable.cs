using UnityEngine;

public class IsClimable : MonoBehaviour
{
    public Climbing climbScript;
    public HandVisibility handVisibility;

    public enum Hand
    {
        Left,
        Right
    }

    public Hand hand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            if (hand == Hand.Left)
            {
                climbScript.SetLeftHandCanClimb(true);
                climbScript.VibrateLeft();
            }
            else
            {
                climbScript.SetRightHandCanClimb(true);
                climbScript.VibrateRight();
            }

            handVisibility.SetTransparent(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            if (hand == Hand.Left)
            {
                climbScript.SetLeftHandCanClimb(false);
            }
            else
            {
                climbScript.SetRightHandCanClimb(false);
            }

            handVisibility.SetTransparent(false);
        }
    }
}