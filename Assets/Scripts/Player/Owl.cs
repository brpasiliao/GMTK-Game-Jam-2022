using UnityEngine;
using System.Collections;

public class Owl : Player {
    public float speed;
    public float speedFlight;
    public float jump;
    public float jumpFlight;
    public float gravUp;
    public float gravUpFlight;
    public int flaps = 4;
    private int currentFlaps;

    // public AudioSource sfx;

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = jump;
        currentFlaps = flaps;
    }

    protected override void Special() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (currentFlaps > 0 || isGrounded) {
                // sfx.Play();

                if (isGrounded) {
                    currentSpeed = speed;
                    currentJump = jump;
                    gravityUp = gravUp;
                    currentFlaps = flaps;
                } else {
                    currentSpeed = speedFlight;
                    currentJump = jumpFlight;
                    gravityUp = gravUpFlight;
                    currentFlaps--;
                }

                rb.velocity = new Vector2 (rb.velocity.x, currentJump);
            }
        }

        // if (player.rb.velocity.y > 0) 
        //     GetComponent<Animator>().Play("Wing_Flap");
        // else 
        //     GetComponent<Animator>().Play("Wing_Idle");
    }

    protected override void EnemyContact(Enemy enemy) {
        if (this.enabled) {
            if (currentFlaps > 0 && Input.GetKeyDown(KeyCode.Space)) {
                enemy.CallDieTemporarily();
                currentFlaps = flaps;
            }
            else if (!isSafe) GetHurt(enemy);
        }
    }
}