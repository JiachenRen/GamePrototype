using System;
using UnityEngine;

namespace Player.Characters
{
    public class UnityChanCharacter : BaseCharacter
    {
        public float forwardSpeed = 5;
        public float backwardSpeed = 3;
        public float runSpeed = 7;
        public float rotationSpeed = 0.2f;
        
        public override void UpdateTransform(Transform t, Vector3 vel)
        {
            var s = controller.accelerating ? runSpeed : forwardSpeed;
            s = vel.y > 0 ? s : backwardSpeed;
            var y = vel.y;
            if (vel.y == 0 && vel.x != 0)
            {
                y = Math.Abs(vel.x);
            }
            t.position += transform.forward * s * y * Time.deltaTime;

            Camera.main!.GetComponent<PlanetaryCameraController>().yawOffset += rotationSpeed * vel.x * Time.deltaTime;
        }

        public override bool ShouldApplyRootMotion()
        {
            return false;
        }
    }
}