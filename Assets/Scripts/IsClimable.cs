using UnityEngine;

public class IsClimable : MonoBehaviour
{
    public Climbing climbScript;
    public enum Hand { Left, Right }
    public Hand hand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            if (hand == Hand.Left)
                climbScript.SetLeftHandCanClimb(true);
            else
                climbScript.SetRightHandCanClimb(true);
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
        }
    }
}
