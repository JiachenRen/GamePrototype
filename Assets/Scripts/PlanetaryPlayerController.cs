using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(GroundCheck))]
public class PlanetaryPlayerController : PlayerController
{
    public Planet planet;
    

    Vector3 up
    {
        get { return (transform.position - planet.transform.position).normalized; }
    }

    // Update is called once per frame
    public void Update()
    {
        var up = this.up;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, up);
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        var lookDirection = Vector3.ProjectOnPlane(ray.direction, up);
        transform.rotation = Quaternion.LookRotation(lookDirection, up);
    }

    public override void OnJump(CallbackContext ctx)
    {
        if (jumpStarted) return;
        jumpStarted = true;
        var force = ForwardForce(jumpForwardForceMultiplier) * (accelerating ? 2 : 1);
        force += up * 20 * rb.mass;
        rb.AddForce(force, ForceMode.Impulse);
        anim.SetBool("Land Animation", true);
        anim.SetTrigger("Jump");
    }
}