using UnityEngine;

public class IsClimable : MonoBehaviour
{
    public Climbing climbScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbScript.SetRightHandCanClimb(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbScript.SetRightHandCanClimb(false);
        }
    }
}
