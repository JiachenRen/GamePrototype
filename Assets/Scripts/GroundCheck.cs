using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool Airborne
    {
        get
        {
            return groundContactCount <= 0;
        }
    }

    private int groundContactCount = 0;

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == Constants.Tags.Ground)
        {
            groundContactCount++;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == Constants.Tags.Ground)
        {
            groundContactCount--;
        }
    }
}
