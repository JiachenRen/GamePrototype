using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private int groundContactCount;

    public bool airborne => groundContactCount <= 0;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tags.Ground)) groundContactCount++;
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tags.Ground)) groundContactCount--;
    }
}