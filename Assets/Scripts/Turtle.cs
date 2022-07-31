using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Turtle : MonoBehaviour {
    public PlayerMovement player;

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

    int facing = 1;
    public bool dashing = false;
    public bool sliding = false;
    public bool charging = false;

    private void OnEnable() {
        currentSpeed = speed;
    }

    private void Update() {
        moveVelocity = 0;

        Special();

        if (!(dashing || sliding)) {
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                moveVelocity = -currentSpeed;
                facing = -1;
            }
            if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                moveVelocity = currentSpeed;
                facing = 1;
            }
        } else {
            if (sliding) {
                if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                    if (facing == 1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                } 
                if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                    if (facing == -1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                }
                if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A) || 
                    Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A))
                    slowDown = 0.1f;
            }
        }

        player.rb.velocity = new Vector2(moveVelocity, player.rb.velocity.y);
    }
    
    private void Special() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            charging = true;
            chargeTimer = 0;
            currentSpeed = 0;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<AudioSource>().Play();
        }

        if (Input.GetKey(KeyCode.Space)) {
            chargeTimer += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            charging = false;
            dashing = true;
            GetComponent<AudioSource>().Stop();

            // 4 ; 0.41 | else 3,10 ; 0.205, 0.67
            if (chargeTimer < chargeTime) 
                slideTime = 0.41f;
            else slideTime = Random.Range(0.205f, 0.67f);
        }

        if (dashing) {
            if (slideTimer < slideTime) {
                currentSpeed = speedSlide;
                moveVelocity = currentSpeed * facing;
                slideTimer += Time.deltaTime;
            } else {
                dashing = false;
                slideTimer = 0;
                sliding = true;
            }
        }

        if (sliding) {
            if (currentSpeed > 1f) {
                currentSpeed -= slowDown;
                moveVelocity = currentSpeed * facing;
            } else {
                if (coolDownTimer < coolDown) {
                    currentSpeed = 0;
                    coolDownTimer += Time.deltaTime;
                } else {
                    player.gameObject.GetComponent<Animator>().Play("Shell_Out"); //player
                    GetComponent<SpriteRenderer>().enabled = true;
                    coolDownTimer = 0f;
                    sliding = false;
                    currentSpeed = speed;
                    slowDown = 0.1f;
                }
            }
        }
    }
}