using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravityField : MonoBehaviour
{
    public List<GameObject> subjects;

    // Acceleration towards center due to gravity
    public float gravitationalPull = 9.8f;

    // Update is called once per frame
    public void Update()
    {
        for (var i = subjects.Count - 1; i >= 0; i--)
        {
            var obj = subjects[i];
            if (obj == null)
            {
                subjects.RemoveAt(i);
                continue;
            }

            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var acc = (transform.position - rb.position).normalized * gravitationalPull;
                rb.AddForce(acc, ForceMode.Acceleration);
            }
        }
    }
}
