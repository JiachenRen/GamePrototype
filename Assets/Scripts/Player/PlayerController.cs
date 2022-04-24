using Audio;
using EventSystem;
using EventSystem.Events;
using Player.Characters;
using Terrain;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(GroundCheck))]
    public class PlayerController : Agent
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int UseLandAnimation = Animator.StringToHash("Landing Animation");
        private static readonly int LandTrigger = Animator.StringToHash("Land");
        private static readonly int VelX = Animator.StringToHash("Vel X");
        private static readonly int VelY = Animator.StringToHash("Vel Y");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int AttackForm = Animator.StringToHash("Attack Form");
        private static readonly int SkillQ = Animator.StringToHash("Skill Q");

        public float jumpForwardForceMultiplier = 6;
        public float jumpUpwardForceMultiplier = 20;
        
        public Slider healthBar;

        public Planet planet;

        public BaseCharacter character;

        [HideInInspector] public bool accelerating;

        [HideInInspector] public Rigidbody rb;

        private int attackIdx;

        private GroundCheck groundCheck;

        private bool jumpStarted;

        private Vector3 vel = Vector3.zero;

        private TerrainType currentTerrain => planet.GetTerrainAt(transform.position);

        private Vector3 up => (transform.position - planet.transform.position).normalized;

        protected override Animator anim => character.anim;

        private bool isPlaying => GameState.instance.playing;

        // Start is called before the first frame update
        public void Start()
        {
            Init();
            groundCheck = GetComponent<GroundCheck>();
            rb = GetComponent<Rigidbody>();
            healthBar.value = currentHealth / health;
            
            planet.GetComponent<GravityField>().subjects.Add(gameObject);

            // Disable all other characters
            SwitchCharacter(character.name);
        }

        public void Update()
        {
            var up = this.up;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, up);
            var ray = Camera.main!.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            var lookDirection = Vector3.ProjectOnPlane(ray.direction, up);
            var t = transform;
            t.rotation = Quaternion.LookRotation(lookDirection, up);
            if (character)
                character.UpdateTransform(t, vel);

            var position = t.position;
            healthBar.value = currentHealth / health;

            if (planet.WaterTooDeep(transform)) 
            {
                // TODO: show a breathe bar 
            }

            Debug.DrawRay(position, t.forward * 10, Color.blue);
            Debug.DrawRay(position, t.right * 10, Color.red);
            Debug.DrawRay(position, t.up * 10, Color.green);
        }

        // - Mark: Character delegates

        public void OnCollisionEnter(Collision collision)
        {
            if (jumpStarted && groundCheck.IsGrounded())
            {
                jumpStarted = false;
                anim.SetTrigger(LandTrigger);
                var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Land, currentTerrain);
                EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, AudioSource);
            }
            HitDetection(collision);
        }

        private Vector3 ForwardForce(float forceMultiplier)
        {
            var t = transform;
            var forward = t.forward;
            var force = forward * jumpForwardForceMultiplier;
            var forcePerp = t.right * jumpUpwardForceMultiplier;
            force = force * vel.y + forcePerp * vel.x;
            return force;
        }

        public void SwitchCharacter(string name)
        {
            if (!isPlaying) return;
            foreach (Transform t in transform)
            {
                var c = t.GetComponent<BaseCharacter>();
                if (c == null) continue;
                if (c.name == name)
                {
                    character = c;
                    c.gameObject.SetActive(true);
                }
                else
                {
                    c.gameObject.SetActive(false);
                }
            }
        }

        // - Mark: InputSystem handles

        public void OnMove(CallbackContext ctx)
        {
            if (!isPlaying) return;
            var vel = ctx.ReadValue<Vector2>();
            this.vel.x = vel.x;
            this.vel.y = vel.y;
            anim.SetFloat(VelX, vel.x);
            anim.SetFloat(VelY, vel.y);
        }

        public void OnAccelerate(CallbackContext ctx)
        {
            if (!isPlaying) return;
            accelerating = ctx.action.IsPressed();
            anim.SetBool(Run, accelerating);
        }

        public void OnJump(CallbackContext ctx)
        {
            if (!isPlaying) return;
            if (jumpStarted) return;
            jumpStarted = true;
            var acc = ForwardForce(jumpForwardForceMultiplier) * (accelerating ? 2 : 1);
            acc += up * jumpUpwardForceMultiplier;
            rb.AddForce(acc * rb.mass, ForceMode.Acceleration);
            anim.SetBool(UseLandAnimation, true);
            anim.SetTrigger(Jump);
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Jump, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, AudioSource);
        }

        public void OnLeftMouseClick(CallbackContext ctx)
        {
            if (!isPlaying) return;
            if (!ctx.performed || !canAttack) return;
            
            // Alternate between left and right punches.
            var form = attackIdx % 2 == 0 ? 2 : 5;
            attackIdx++;
            anim.SetFloat(AttackForm, form);
            Attack();
        }

        public void OnSkillQ(CallbackContext ctx)
        {
            if (!isPlaying) return;
            if (ctx.started && canAttack) anim.SetTrigger(SkillQ);
        }

        public void OnFootStep()
        {
            var action = accelerating ? AudioAction.RunStep : AudioAction.WalkStep;
            var audioInfo = new AudioSourceInfo(AudioActor.Player, action, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, AudioSource);
        }

        public void OnLand()
        {
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Land, currentTerrain);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, AudioSource);
        }

        public void OnHit()
        {
            var audioInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.Attack, TerrainType.All);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(audioInfo, AudioSource);
        }

        protected override void GetHit(Agent attacker)
        {
            base.GetHit(attacker);
            var info = new AudioSourceInfo(AudioActor.Player, AudioAction.GetHit, TerrainType.All);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(info, AudioSource);
        }

        protected override void Die()
        {
            base.Die();
            var info = new AudioSourceInfo(AudioActor.Player, AudioAction.Die, TerrainType.All);
            EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(info, AudioSource);
            EventManager.TriggerEvent<DefeatEvent>();
        }
    }
}