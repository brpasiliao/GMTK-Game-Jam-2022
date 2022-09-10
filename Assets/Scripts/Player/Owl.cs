using UnityEngine;
using System.Collections;

public class Owl : Player {
    public float speed;
    public float speedFlight;
    public float jump;
    public float jumpFlight;
    public float fall;
    public float fallFlight;
    public int flaps = 4;

    // public AudioSource sfx;

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = jump;
    }

    protected override void Special() {
        // Debug.Log("owl special");
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (flaps > 0 || isGrounded) {
                // sfx.Play();

                if (isGrounded) {
                    currentSpeed = speed;
                    currentJump = jump;
                    rb.gravityScale = fall;
                    flaps = 4;
                } else {
                    currentSpeed = speedFlight;
                    currentJump = jumpFlight;
                    rb.gravityScale = fallFlight;
                    flaps--;
                }

                rb.velocity = new Vector2 (rb.velocity.x, currentJump);
            }
        }

        // if (player.rb.velocity.y > 0) 
        //     GetComponent<Animator>().Play("Wing_Flap");
        // else 
        //     GetComponent<Animator>().Play("Wing_Idle");
    }

    protected override void EnemyContact(GameObject enemy) {
        if (this.enabled) {
            if (flaps > 0 && Input.GetKeyDown(KeyCode.Space))
                enemy.SetActive(false);
            else GetHurt(enemy);
        }
    }
}