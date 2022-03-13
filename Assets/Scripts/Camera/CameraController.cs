using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float pitchFreedom = 8;
    public float yawSpeed = 90;
    public float pitchAdjustment = -25;

    [Tooltip("How many meters above the head of player to place the camera.")]
    public float elevation = 5;

    [Tooltip("How far away from the player to place the camera.")]
    public float distance = 10;

    // Vertial rotation offset
    protected float pitchOffset;

    private bool playMode = true;

    // Horizontal rotation offset
    protected float yawOffset;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        var rotation = Quaternion.Euler(0, yawOffset * yawSpeed, 0);
        var positionOffset = new Vector3(-rotation.y * distance, elevation, -rotation.w * distance);
        transform.position = player.transform.position + positionOffset;
        transform.LookAt(player.transform.position);
        var r = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(r.x + pitchAdjustment - pitchFreedom * pitchOffset, r.y, r.z);
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