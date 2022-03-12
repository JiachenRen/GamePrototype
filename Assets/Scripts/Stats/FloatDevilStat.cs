using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDevilStat : MonoBehaviour {

    public float HP = 100f;
    public GameObject player;
    private Animator anim;
    private Animator playerAnim;
    private bool hitable = true;


    // Start is called before the first frame update
    void Start() {
        anim = GetComponentInChildren<Animator>();
        playerAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
            hitable = true;
        }
        if (HP <= 0) {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other) {
        HitDetection(other);
    }

    private void OnTriggerStay(Collider other) {
        HitDetection(other);
    }

    void HitDetection(Collider other) {
        bool isAttacking = playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        if (isAttacking && hitable && other.gameObject.tag == "Weapon") {
            if (other.gameObject.name == "BodyGuard_03_Hand") {
                print("player hit me with hand, -10HP");
                HP -= 10;
            }
            anim.SetTrigger("getHit");
            hitable = false;
        }
    }

    void Die() {
        anim.SetTrigger("die");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("dead")) {
            Destroy(gameObject);
        }
    }
}
