using System.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;
using Image = UnityEngine.UIElements.Image;

[ExecuteInEditMode]
public class MinimapCameraController : MonoBehaviour
{
    public GameObject player;
    public RawImage directionIndicator;
    public GameObject compass;
    public float elevation;
    
    // Either orient to the north or according to player's orientation
    public bool northUp;

    private void LateUpdate()
    {
        // Update camera
        var playerPos = player.transform.position;
        transform.position = playerPos + player.transform.up * elevation;
        var camUp = northUp ? Vector3.up : player.transform.forward;
        transform.LookAt(playerPos, camUp);

        // Update player direction indicator
        var playerUp = player.transform.up;
        var playerForward = player.transform.forward;
        var projUp = Vector3.ProjectOnPlane(camUp, playerUp);
        var projForward = Vector3.ProjectOnPlane(playerForward, playerUp);
        var rot = Quaternion.FromToRotation(projForward, projUp);
        directionIndicator.rectTransform.rotation = Quaternion.identity;
        directionIndicator.rectTransform.Rotate(new Vector3(0, 0, rot.eulerAngles.y));
        
        // Update compass orientation
        compass.transform.rotation = Quaternion.identity;
        if (!northUp)
        {
            var angle = Vector3.SignedAngle(
                Vector3.ProjectOnPlane(Vector3.up, playerUp),
                Vector3.ProjectOnPlane(player.transform.forward, playerUp),
                playerUp
            );
            compass.transform.Rotate(0, 0, angle);
        }
    }

    public void ToggleNorthUp()
    {
        northUp = !northUp;
    }
}