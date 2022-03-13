using UnityEngine;

[ExecuteInEditMode]
public class PlanetaryCameraController : CameraController
{
    public void LateUpdate()
    {
        var playerRot = player.transform.rotation;
        var camRot = Quaternion.AngleAxis(yawOffset * yawSpeed, player.transform.up);
        var camForward = Vector3.ProjectOnPlane(camRot * Vector3.forward, player.transform.up).normalized;
        var camPosition = player.transform.position + player.transform.up * elevation - camForward * distance;
        transform.position = camPosition;
        transform.LookAt(
            player.transform.position + player.transform.up * (pitchAdjustment + pitchFreedom * pitchOffset),
            player.transform.up);
    }
}