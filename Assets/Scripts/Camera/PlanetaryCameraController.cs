using UnityEngine;

[ExecuteInEditMode]
public class PlanetaryCameraController : CameraController
{
    public void LateUpdate()
    {
        var playerUp = player.transform.up;
        var playerPos = player.transform.position;

        var camRot = Quaternion.AngleAxis(YawOffset * yawSpeed, playerUp);
        var camForward = Vector3.ProjectOnPlane(camRot * Vector3.forward, playerUp).normalized;
        var camPosition = playerPos + playerUp * elevation - camForward * distance;
        transform.position = camPosition;
        transform.LookAt(
            playerPos + playerUp * (pitchOffset + pitchFreedom * PitchOffset),
            playerUp);
    }
}