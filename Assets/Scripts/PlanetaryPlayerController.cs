using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(GroundCheck))]
public class PlanetaryPlayerController : PlayerController
{
    public Planet planet;

    private Vector3 up => (transform.position - planet.transform.position).normalized;

    public override void Start()
    {
        base.Start();

        planet.GetComponent<GravityField>().subjects.Add(gameObject);
    }

    // Update is called once per frame
    public void Update()
    {
        var up = this.up;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, up);
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        var lookDirection = Vector3.ProjectOnPlane(ray.direction, up);
        transform.rotation = Quaternion.LookRotation(lookDirection, up);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 10, Color.red);
        Debug.DrawRay(transform.position, transform.up * 10, Color.green);
    }

    protected override Vector3 ForwardForce(float forceMultiplier)
    {
        var forward = transform.forward;
        var force = forward * rb.mass * jumpForwardForceMultiplier;
        var forcePerp = transform.right * rb.mass * jumpUpwardForceMultiplier;
        force = force * vel.y + forcePerp * vel.x;
        return force;
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