using UnityEngine;
using System.Collections;

public class Owl : MonoBehaviour {
    public Player player;

    float moveVelocity;
    float currentSpeed;
    float currentJump;

    public float speed;
    public float speedFlight;
    public float jump;
    public float jumpFlight;
    public float fall;
    public float fallFlight;
    public int flaps = 4;

    // BoxCollider2D playerCollider;
    // Vector2 colliderSize;

    // private void Start() {
    //     playerCollider = player.GetComponent<BoxCollider2D>();
    //     colliderSize = playerCollider.size;
    // }

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = jump;
    }

    private void Update() {
        moveVelocity = 0;

        Special();

        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
            moveVelocity = -currentSpeed;
        if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
            moveVelocity = currentSpeed;

        if (player.isHit) moveVelocity = -5 * player.facing;

        player.rb.velocity = new Vector2(moveVelocity, player.rb.velocity.y);
    }

    private void Special() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (flaps > 0 || player.isGrounded) {
                GetComponent<AudioSource>().Play();

                if (player.isGrounded) {
                    currentSpeed = speed;
                    currentJump = jump;
                    player.rb.gravityScale = fall;
                    flaps = 4;
                } else {
                    currentSpeed = speedFlight;
                    currentJump = jumpFlight;
                    player.rb.gravityScale = fallFlight;
                    flaps--;

                    // playerCollider.size = new Vector2 (playerCollider.size.x * 1.5f, playerCollider.size.y * 1.5f);
                }

                player.rb.velocity = new Vector2 (player.rb.velocity.x, currentJump);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            // playerCollider.size = colliderSize;
        }

        if (player.rb.velocity.y > 0) 
            GetComponent<Animator>().Play("Wing_Flap");
        else 
            GetComponent<Animator>().Play("Wing_Idle");
    }
}