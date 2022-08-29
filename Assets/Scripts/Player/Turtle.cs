using UnityEngine;
using System.Collections;

public class Turtle : MonoBehaviour {
    public Player player;

    float moveVelocity;
    float currentSpeed;

    public float speed;
    public float speedSlide;
    public float slideTime;
    float slideTimer = 0;
    
    public float slowDown; // natural slowdown
    public float slowDown2; // forced slowdown
    public float chargeTime;
    float chargeTimer = 0;
    public float coolDown;
    float coolDownTimer;

    public bool isDashing = false;
    public bool isSliding = false;
    public bool isCharging = false;
    public int direction = 1;

    private void OnEnable() {
        currentSpeed = speed;
    }

    private void Update() {
        moveVelocity = 0;

        Special();

        if (!(isDashing || isSliding)) {
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                moveVelocity = -currentSpeed;
                direction = -1;
            }
            if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                moveVelocity = currentSpeed;
                direction = 1;
            }

        } else {
            if (isSliding) {
                if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                    if (direction == 1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                } 
                if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                    if (direction == -1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                }
                if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A) || 
                    Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D))
                    slowDown = 0.1f;
            }
        }

        if (player.isHit) {
            moveVelocity = -5 * player.facing;
            ResetValues();
        }

        player.rb.velocity = new Vector2(moveVelocity, player.rb.velocity.y);
    }
    
    private void Special() {
        if (!(isDashing || isSliding)) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                isCharging = true;
                chargeTimer = 0;
                currentSpeed = 0;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<AudioSource>().Play();
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTimer += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                isCharging = false;
                GetComponent<AudioSource>().Stop();

                if (chargeTimer > chargeTime) isDashing = true;
                else ResetValues();
            }
        }

        if (isDashing) {
            if (slideTimer < slideTime) {
                currentSpeed = speedSlide;
                moveVelocity = currentSpeed * direction;
                slideTimer += Time.deltaTime;
            } else {
                isDashing = false;
                slideTimer = 0;
                isSliding = true;
            }
        }

        if (isSliding) {
            if (currentSpeed > 1f) {
                currentSpeed -= slowDown;
                moveVelocity = currentSpeed * direction;
            } else {
                if (coolDownTimer < coolDown) {
                    currentSpeed = 0;
                    coolDownTimer += Time.deltaTime;
                } else {
                    player.gameObject.GetComponent<Animator>().Play("Shell_Out"); //player
                    GetComponent<SpriteRenderer>().enabled = true;
                    coolDownTimer = 0f;
                    isSliding = false;
                    currentSpeed = speed;
                    slowDown = 0.1f;
                }
            }
        }
    }

    private void ResetValues() {
        isDashing = false;
        isSliding = false;

        slideTimer = 0f;
        coolDownTimer = 0f;
        slowDown = 0.1f;
        currentSpeed = speed;

        GetComponent<SpriteRenderer>().enabled = true;
    }
}