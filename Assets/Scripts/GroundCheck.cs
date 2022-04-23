using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public string checkAgainst = Constants.Tags.TerrainSurface;
    public float tolerance = 0.1f;
    public bool IsGrounded()
    {
        var t = transform;
        var hit = Physics.Raycast(t.position, -t.up, out var hitInfo,  tolerance, Physics.AllLayers);
        return hit && hitInfo.collider.CompareTag(checkAgainst);
    }
}