using UnityEngine;

namespace Player.Characters
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(CapsuleCollider))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public string characterName;

        [HideInInspector] public Animator anim;

        [HideInInspector] public Rigidbody rb;

        protected PlayerController controller;

        private void Start()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            controller = transform.parent.GetComponent<PlayerController>();
            if (!controller)
                Debug.LogError("Character must be a child of PlayerController");
        }

        public virtual bool ShouldApplyRootMotion()
        {
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            return !stateInfo.IsName("Idle") && !stateInfo.IsTag("No Root Motion");
        }
        
        public bool CanAttack()
        {
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsTag("Idle") && !anim.IsInTransition(0);
        }

        public void OnAnimatorMove()
        {
            if (!ShouldApplyRootMotion()) return;
            controller.rb.MovePosition(anim.rootPosition);
            controller.rb.MoveRotation(anim.rootRotation);
        }

        public void FootL()
        {
            controller.OnFootStep();
        }

        public void FootR()
        {
            controller.OnFootStep();
        }
        
        public void Land()
        {
            controller.OnLand();
        }

        public void Hit()
        {
            controller.OnHit();
        }

        public void OnCollisionEnter(Collision collision)
        {
            controller.OnCollisionEnter(collision);
        }

        public abstract void UpdateTransform(Transform t, Vector3 vel);
    }
}