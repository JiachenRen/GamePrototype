using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private int groundContactCount;

    public bool Airborne => groundContactCount <= 0;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Constants.Tags.Ground) groundContactCount++;
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == Constants.Tags.Ground) groundContactCount--;
    }
}