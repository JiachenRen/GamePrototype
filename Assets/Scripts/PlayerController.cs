using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    public float jumpForceMultiplier = 3;
    public Camera mainCamera;

    private Animator anim;
    private Rigidbody rb;
    private GroundCheck check;

    private bool jumpStarted = false;
    private bool accelerating = false;
    private Vector3 vel = Vector3.zero;

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

    private void OnAnimatorMove()
    {
        rb.MovePosition(anim.rootPosition);
        rb.MoveRotation(anim.rootRotation);
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

    public void OnJump(CallbackContext ctx)
    {
        if (jumpStarted) return;
        jumpStarted = true;
        var forward = transform.forward;
        var force = new Vector3(forward.x * rb.mass * jumpForceMultiplier, forward.y, forward.z * rb.mass * jumpForceMultiplier);
        var forcePerp = new Vector3(force.z, force.y, -force.x);
        Debug.DrawRay(transform.position, force, Color.red, 2);
        force = force * vel.y + forcePerp * vel.x;
        if (accelerating)
        {
            force.Scale(new Vector3(2f, 0, 2f));
        }
        force.y += 10 * rb.mass;
        rb.AddForce(force, ForceMode.Impulse);
        anim.SetTrigger("Jump");
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
    
    public void OnCollisionEnter(Collision collision)
    {

        if (jumpStarted && !check.Airborne) 
        { 
            jumpStarted = false;
            anim.SetTrigger("Land");
        }
    }
}
