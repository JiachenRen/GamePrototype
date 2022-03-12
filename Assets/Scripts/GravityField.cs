using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravityField : MonoBehaviour
{
    public List<GameObject> objects;

    // Acceleration towards center due to gravity
    public float gravitationalPull = 9.8f;

    // Update is called once per frame
    public void Update()
    {
        foreach (var obj in objects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var acc = (transform.position - rb.position).normalized * gravitationalPull;
                rb.AddForce(acc, ForceMode.Acceleration);
            }
        }
    }
}
