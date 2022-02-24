using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class CameraController : MonoBehaviour
{

    public GameObject player;

    public float pitchSpeed = 8;
    public float yawSpeed = 90;
    public float pitchAdjustment = -15;

    [Tooltip("How many meters above the head of player to place the camera.")]
    public float elevation = 3;

    private bool playMode = true;

    // Horizontal rotation offset
    private float yawOffset = 0;

    // Vertial rotation offset
    private float pitchOffset = 0;

    private Vector2? lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var rotation = Quaternion.Euler(0, yawOffset * yawSpeed, 0);
        var positionOffset = new Vector3(-rotation.y * 5, elevation, -rotation.w * 5);
        transform.position = player.transform.position + positionOffset;
        transform.LookAt(player.transform.position);
        var r = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(r.x + pitchAdjustment - pitchSpeed * pitchOffset, r.y, r.z);
    }

    public void OnMouseDelta(CallbackContext ctx)
    {
        if (!playMode) return;
        var delta = ctx.ReadValue<Vector2>();
        yawOffset += delta.x / Screen.width;
        pitchOffset += delta.y / Screen.height;
        pitchOffset = Mathf.Clamp(pitchOffset, 0, 1);
    }

    public void OnEscape(CallbackContext ctx)
    {
        playMode = !playMode;
        Cursor.lockState = playMode ? CursorLockMode.Locked : CursorLockMode.None;
    }  
}
