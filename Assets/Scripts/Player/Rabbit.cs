using UnityEngine;
using System.Collections;

public class Rabbit : Player {
    public float speed;
    public float jump;
    public float jumpBoost;

    public float chargeTime;
    float chargeTimer = 0;

    // public AudioSource sfx;

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = jump;

        GetComponent<Animator>().Play("Rabbit_Idle");
    }

    protected override void Special() {
        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                chargeTimer = 0;
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTimer += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                if (chargeTimer > chargeTime) {
                    currentJump = jump;

                    // sfx.Play();
                } 
                else currentJump = 0;

                rb.velocity = new Vector2 (rb.velocity.x, currentJump);
            }
        }
    }

    protected override void EnemyContact(Enemy enemy) {
        if (this.enabled) {
            if (rb.velocity.y < 0f) {
                enemy.CallDieTemporarily();
                currentJump += jumpBoost;
                rb.velocity = new Vector2 (rb.velocity.x, currentJump);
            }
            else if (!isSafe) GetHurt(enemy);
        }
    }
}