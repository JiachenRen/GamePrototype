using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject cam;
    public GameObject player;
    
    public void LateUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
}
