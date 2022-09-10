using UnityEngine;
using System.Collections;

public class Rabbit : Player {
    public float speed;
    public float jump;

    public float chargeTime;
    float chargeTimer = 0;

    // public AudioSource sfx;

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = 0;
    }

    protected override void Special() {
        // Debug.Log("rabbit special");
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

                rb.velocity = new Vector2 (rb.velocity.x, currentJump);
                currentJump = 0;
            }
        }
    }

    protected override void EnemyContact(GameObject enemy) {
        if (this.enabled) {
            if (rb.velocity.y < 0f) enemy.SetActive(false);
            else GetHurt(enemy);
        }
    }
}