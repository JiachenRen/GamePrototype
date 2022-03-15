using Audio;
using EventSystem;
using EventSystem.Events;
using Terrain;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    [RequireComponent(typeof(GroundCheck))]
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int UseLandAnimation = Animator.StringToHash("Land Animation");
        private static readonly int LandTrigger = Animator.StringToHash("Land");
        private static readonly int VelX = Animator.StringToHash("Vel X");
        private static readonly int VelY = Animator.StringToHash("Vel Y");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int AttackForm = Animator.StringToHash("Attack Form");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int SkillQ = Animator.StringToHash("Skill Q");

        public float jumpForwardForceMultiplier = 6;
        public float jumpUpwardForceMultiplier = 20;

        public Planet planet;

        private bool accelerating;

        private Animator anim;

        private int attackIdx;

        private GroundCheck groundCheck;

        private bool jumpStarted;

        private Rigidbody rb;

        private bool stasis = true;

        private Vector3 vel = Vector3.zero;

        private AudioSource audioSource;

        private TerrainType currentTerrain => planet.GetTerrainAt(transform.position);

        private Vector3 up => (transform.position - planet.transform.position).normalized;

        // Start is called before the first frame update
        public void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            groundCheck = GetComponent<GroundCheck>();
            audioSource = GetComponent<AudioSource>();

            // Enter stasis until ground contact.
            anim.speed = 0f;

            planet.GetComponent<GravityField>().subjects.Add(gameObject);
        }

        public void Update()
        {
            var up = this.up;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, up);
            var ray = Camera.main!.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            var lookDirection = Vector3.ProjectOnPlane(ray.direction, up);
            transform.rotation = Quaternion.LookRotation(lookDirection, up);

            var position = transform.position;
            Debug.DrawRay(position, transform.forward * 10, Color.blue);
            Debug.DrawRay(position, transform.right * 10, Color.red);
            Debug.DrawRay(position, transform.up * 10, Color.green);
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

        public void OnCollisionEnter(Collision collision)
        {
            // Contact with ground, start spawn animation.
            if (!groundCheck.airborne && stasis)
                if (stasis)
                {
                    stasis = false;
                    anim.speed = 1;
                }

            if (jumpStarted && !groundCheck.airborne)
            {
                jumpStarted = false;
                anim.SetTrigger(LandTrigger);
                var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Land, currentTerrain);
                EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, audioSource);
            }
        }

        public void OnMove(CallbackContext ctx)
        {
            var vel = ctx.ReadValue<Vector2>();
            this.vel.x = vel.x;
            this.vel.y = vel.y;
            anim.SetFloat(VelX, vel.x);
            anim.SetFloat(VelY, vel.y);
        }

        public void OnAccelerate(CallbackContext ctx)
        {
            accelerating = ctx.action.IsPressed();
            anim.SetBool(Run, accelerating);
        }

        public void OnJump(CallbackContext ctx)
        {
            if (jumpStarted) return;
            jumpStarted = true;
            var force = ForwardForce(jumpForwardForceMultiplier) * (accelerating ? 2 : 1);
            force += up * 20 * rb.mass;
            rb.AddForce(force, ForceMode.Impulse);
            anim.SetBool(UseLandAnimation, true);
            anim.SetTrigger(Jump);
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Jump, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, audioSource);
        }

        public void OnLeftMouseClick(CallbackContext ctx)
        {
            if (ctx.performed && CanAttack())
            {
                // Alternate between left and right punches.
                var form = attackIdx % 2 == 0 ? 2 : 5;
                attackIdx++;
                anim.SetFloat(AttackForm, form);
                anim.SetTrigger(Attack);
            }
        }

        public void OnSkillQ(CallbackContext ctx)
        {
            if (ctx.started && CanAttack()) anim.SetTrigger(SkillQ);
        }

        private Vector3 ForwardForce(float forceMultiplier)
        {
            var t = transform;
            var forward = t.forward;
            var mass = rb.mass;
            var force = forward * mass * jumpForwardForceMultiplier;
            var forcePerp = t.right * mass * jumpUpwardForceMultiplier;
            force = force * vel.y + forcePerp * vel.x;
            return force;
        }

        private bool CanAttack()
        {
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsTag("Idle") && !anim.IsInTransition(0);
        }

        private void FootStep()
        {
            var action = accelerating ? AudioAction.RunStep : AudioAction.WalkStep;
            var audioInfo = new AudioSourceInfo(AudioActor.Player, action, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, audioSource);
        }

        private void FootL()
        {
            FootStep();
        }

        private void FootR()
        {
            FootStep();
        }

        private void Land()
        {
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Land, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, audioSource);
        }

        private void Hit()
        {
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Attack, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, audioSource);
        }
    }
}