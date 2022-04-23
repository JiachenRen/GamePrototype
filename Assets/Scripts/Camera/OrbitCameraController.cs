using System;
using Terrain;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitCameraController : MonoBehaviour
{
    public Planet planet;

    public float yaw;
    public float pitch;
    public float height;

    public float forwardSpeed = 1;
    public float horizontalSpeed = 1;

    public void OnValidate()
    {
        Init();
    }

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        var t = transform;
        t.rotation = Quaternion.identity;
        t.position = Vector3.up;
    }

    public void LateUpdate()
    {
        var t = transform;
        var planetPos = planet.transform.position;
        var up = (t.position - planetPos).normalized;
        var pos = up * (planet.radius + height);
        t.position = pos;
        t.rotation = Quaternion.FromToRotation(Vector3.up, up);
        t.RotateAround(pos, up, pitch);
        t.RotateAround(pos, t.right, yaw);
        t.RotateAround(planetPos, t.right, forwardSpeed * Time.deltaTime);
        // t.RotateAround(planetPos, t.forward, horizontalSpeed * Time.deltaTime);
    }
}