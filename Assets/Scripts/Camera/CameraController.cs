using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float pitchFreedom = 8;
    public float yawSpeed = 90;
    public float pitchOffset = -25;

    [Range(0, 1f)] public float displacementSpeed = 0.2f;
    public Vector2 elevationRange = new Vector2(1f, 9f);
    public Vector2 distanceRange = new Vector2(1f, 11.5f);
    [Range(0, 1)] public float relativeElevation = 0.5f;
    [Range(0, 1)] public float relativeDistance = 0.5f;


    protected float elevation => Mathf.Lerp(elevationRange.x, elevationRange.y, relativeElevation);
    protected float distance => Mathf.Lerp(distanceRange.x, distanceRange.y, relativeDistance);

    // Vertical rotation offset
    protected float PitchOffset;

    private bool playMode = true;

    // Horizontal rotation offset
    protected float YawOffset;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        var rotation = Quaternion.Euler(0, YawOffset * yawSpeed, 0);
        var positionOffset = new Vector3(-rotation.y * distance, elevation, -rotation.w * distance);
        var position = player.transform.position;
        transform.position = position + positionOffset;
        transform.LookAt(position);
        var r = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(r.x + pitchOffset - pitchFreedom * PitchOffset, r.y, r.z);
    }

    public void OnMouseDelta(CallbackContext ctx)
    {
        if (!playMode) return;
        var delta = ctx.ReadValue<Vector2>();
        YawOffset += delta.x / Screen.width;
        PitchOffset += delta.y / Screen.height;
        PitchOffset = Mathf.Clamp(PitchOffset, 0, 1);
    }

    public void OnMouseScroll(CallbackContext ctx)
    {
        var deltaY = ctx.ReadValue<Vector2>().y;
        var dir = new Vector2(distanceRange.y - distanceRange.x, elevationRange.y - elevationRange.x).normalized;
        var displacement = dir * deltaY * displacementSpeed / 10 * Time.deltaTime;
        relativeDistance += displacement.x;
        relativeDistance = Mathf.Clamp(relativeDistance, 0, 1);
        relativeElevation += displacement.y;
        relativeElevation = Mathf.Clamp(relativeElevation, 0, 1);
    }

    public void OnEscape(CallbackContext ctx)
    {
        playMode = !playMode;
        Cursor.lockState = playMode ? CursorLockMode.Locked : CursorLockMode.None;
    }
}