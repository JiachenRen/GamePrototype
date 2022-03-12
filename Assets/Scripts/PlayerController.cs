using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    public float jumpForwardForceMultiplier = 3;
    public float jumpUpwardForceMultiplier = 10;
    public Camera mainCamera;

    protected Animator anim;

    protected Rigidbody rb;

    protected GroundCheck check;

    protected bool jumpStarted = false;

    protected bool accelerating = false;

    protected Vector3 vel = Vector3.zero;

    protected int attackIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        check = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        var anchor = ray.GetPoint(10);
        transform.LookAt(anchor);
        var r = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, r.y , r.z);
    }

    public void OnAnimatorMove()
    {
        var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsTag("No Root Motion"))
        {
            rb.MovePosition(anim.rootPosition);
            rb.MoveRotation(anim.rootRotation);
        }
    }

    public void OnMove(CallbackContext ctx)
    {
        var vel = ctx.ReadValue<Vector2>();
        this.vel.x = vel.x;
        this.vel.y = vel.y;
        anim.SetFloat("Vel X", vel.x);
        anim.SetFloat("Vel Y", vel.y);
    }

    public void OnAccelerate(CallbackContext ctx)
    {
        accelerating = ctx.action.IsPressed();
        anim.SetBool("Run", accelerating);
    }

    public virtual void OnJump(CallbackContext ctx)
    {
        if (jumpStarted) return;
        jumpStarted = true;
        var force = ForwardForce(jumpForwardForceMultiplier);
        if (accelerating)
        {
            force.Scale(new Vector3(2f, 0, 2f));
        }
        force.y += jumpUpwardForceMultiplier * rb.mass;
        rb.AddForce(force, ForceMode.Impulse);
        anim.SetBool("Land Animation", true);
        anim.SetTrigger("Jump");
    }

    public void OnLeftMouseClick(CallbackContext ctx)
    {
        if (ctx.performed && CanPerformAction())
        {
            // Alternate between left and right punches.
            var form = (attackIdx % 2) == 0 ? 2 : 5;
            attackIdx++;
            anim.SetFloat("Attack Form", form);
            anim.SetTrigger("Attack");
        }
    }

    public void OnSkillQ(CallbackContext ctx)
    {
        if (ctx.started && CanPerformAction())
        {
            anim.SetTrigger("Skill Q");
        }
    }

    protected Vector3 ForwardForce(float forceMultiplier)
    {
        var forward = transform.forward;
        var force = new Vector3(forward.x * rb.mass * forceMultiplier, forward.y, forward.z * rb.mass * forceMultiplier);
        var forcePerp = new Vector3(force.z, force.y, -force.x);
        force = force * vel.y + forcePerp * vel.x;
        return force;
    }

    public bool CanPerformAction()
    {
        var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Idle") && !anim.IsInTransition(0);
    }

    void FootL()
    {

    }

    void FootR()
    {

    }

    void Land()
    {

    }

    void Hit()
    {

    }
    
    public void OnCollisionEnter(Collision collision)
    {

        if (jumpStarted && !check.Airborne) 
        { 
            jumpStarted = false;
            anim.SetTrigger("Land");
        }
    }
}
