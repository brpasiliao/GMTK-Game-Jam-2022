using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rabbit : MonoBehaviour {
    public PlayerMovement player;

    float moveVelocity;
    float currentSpeed;
    float currentJump;

    public float speed;
    float jump = 0; 
    public float jumpNormal;
    public float jumpHigh;

    public float chargeTime;
    float chargeTimer = 0;

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

        player.rb.velocity = new Vector2(moveVelocity, player.rb.velocity.y);
    }

    private void Special() {
        if (player.isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                chargeTimer = 0;
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTimer += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                if (chargeTimer > chargeTime) {
                    if (Random.Range(0,2) == 0) currentJump = jumpNormal;
                    else currentJump = jumpHigh;

                    GetComponent<AudioSource>().Play();
                }

                player.rb.velocity = new Vector2 (player.rb.velocity.x, currentJump);
                currentJump = jump;
            }
        }
    }
}