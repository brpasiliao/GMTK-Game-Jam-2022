using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public string character;

    public float rabbitSpeed = 7f;
    public float rabbitJump = 29f;
    public bool marker;

    public float turtleSpeed = 3f;
    public float turtleSpeedSlide = 15f;
    public float turtleJump = 0;
    public float slideTime = 0;
    public float maxSlideTime = 0.41f;
    public float coolDown;
    public float coolDownTimer;
    public bool dashing = false;
    public bool sliding = false;

    public float owlSpeed = 6f;
    public float owlSpeedFlight = 7f;
    public float owlJump = 26f;
    public float owlJumpFlight = 6f;
    public int flaps = 4;
    public bool marker2;

    public float minChargeTime = 1f;
    public float chargeTime = 0;
    public bool charging = false;

    //Movement
    public float speed;
    public float jump;
    public float slowDown; // natural slowdown
    public float slowDown2; // forced slowdown
    public float slopeMultiplier;
    float moveVelocity;
    int facing;

    //Grounded Vars
    bool isGrounded = true;
    bool onSlope = false;

    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        changeCharacter("rabbit");
    }

    void Update () {
        moveVelocity = 0;

        Special();

        //Left Right Movement
        if (character == "turtle" && (dashing || sliding)) {
            if (sliding) {
                if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                    if (facing == 1) slowDown *= slowDown2;
                    else slowDown = 0;
                } 
                if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                    if (facing == -1) slowDown *= slowDown2;
                    else slowDown = 0;
                }
                if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A) || 
                    Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A))
                    slowDown = 0.5f;
            }
        } else {
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                moveVelocity = -speed;
                facing = -1;
            }
            if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                moveVelocity = speed;
                facing = 1;
            }
        }

        if (onSlope) rb.velocity = new Vector2 (moveVelocity, rb.velocity.y * slopeMultiplier);
        else rb.velocity = new Vector2 (moveVelocity, rb.velocity.y);

        if (Input.GetKeyDown("1")) changeCharacter("rabbit");
        if (Input.GetKeyDown("2")) changeCharacter("turtle");
        if (Input.GetKeyDown("3")) changeCharacter("owl");
    }

    void Special() {
        if (character == "rabbit" && isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                charging = true;
                chargeTime = 0;
                speed = 0;
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                charging = false;

                if (chargeTime > minChargeTime) {
                    jump = Random.Range(21f, 38f);
                }

                speed = rabbitSpeed;
                rb.velocity = new Vector2 (rb.velocity.x, jump);
                jump = rabbitJump;
            }
        }

        else if (character == "owl" && Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded) {
                speed = owlSpeed;
                jump = owlJump;
                rb.gravityScale = 6;

                rb.velocity = new Vector2 (rb.velocity.x, jump);
                // if (Random.Range(0,2) == 0) flaps = 1;
                // else flaps = 4;
                flaps = 4;

            } else {
                speed = owlSpeedFlight;
                jump = owlJumpFlight;

                if (flaps > 0) {
                    rb.velocity = new Vector2 (rb.velocity.x, jump);
                    flaps--;
                } else {
                    rb.gravityScale = 10;
                }
            }

        } else if (character == "turtle") {
            if (Input.GetKeyDown(KeyCode.Space)) {
                charging = true;
                chargeTime = 0;
                speed = 0;
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                charging = false;
                dashing = true;

                // 4 ; 0.41 | else 3,10 ; 0.205, 0.67
                if (chargeTime < minChargeTime) 
                    maxSlideTime = 0.41f;
                else maxSlideTime = Random.Range(0.205f, 0.67f);
            }

            if (dashing) {
                if (slideTime < maxSlideTime) {
                    speed = turtleSpeedSlide;
                    moveVelocity = speed * facing;
                    slideTime += Time.deltaTime;
                } else {
                    dashing = false;
                    slideTime = 0;
                    // speed = turtleSpeed;
                    sliding = true;
                }
            }

            if (sliding) {
                if (speed > 1f) {
                    speed -= slowDown;
                    moveVelocity = speed * facing;
                } else {
                    if (coolDownTimer < coolDown) {
                        speed = 0;
                        coolDownTimer += Time.deltaTime;
                    } else {
                        coolDownTimer = 0f;
                        sliding = false;
                        speed = turtleSpeed;
                        slowDown = 0.5f;
                    }
                }
            }
        }
    }

    // Check if Grounded
    // cannot touch 2 at the same time
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
        if (collision.gameObject.tag == "Slope")
            onSlope = true;
        if (collision.gameObject.tag == "Wall" &&
            character == "turtle" && (dashing || sliding)) {
                facing = -facing;
                // slowDown = -slowDown;
            }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
        if (collision.gameObject.tag == "Slope")
            onSlope = false;
    }

    void changeCharacter(string c) {
        if (c == "rabbit") {
            character = "rabbit";
            GetComponent<SpriteRenderer>().color = Color.blue;
            speed = rabbitSpeed;
            jump = rabbitJump;

        } else if (c == "turtle") {
            character = "turtle";
            GetComponent<SpriteRenderer>().color = Color.green;
            speed = turtleSpeed;
            jump = turtleJump;

        } else if (c == "owl") {
            character = "owl";
            GetComponent<SpriteRenderer>().color = Color.red;
            speed = owlSpeed;
            jump = owlJump;
        }
    }
}