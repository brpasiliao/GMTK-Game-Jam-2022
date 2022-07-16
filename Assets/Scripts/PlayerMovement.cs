// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(CapsuleCollider2D))]

// public class PlayerMovement : MonoBehaviour {
//     // Move player in 2D space
//     public float rMaxSpeed;
//     public float rJumpHeight;

//     public float tMaxSpeed;
//     public float tSlideSpeed;
//     public float tJumpHeight;
//     public float minChargeTime;
//     public float chargeTime = 0;
//     public bool charging = false;
//     public float slideTime = 0;
//     // public float minSlideTime;
//     public float maxSlideTime;
//     public bool sliding = false;

//     public float oMaxSpeed;
//     public float oFlightSpeed;
//     public float oJumpHeight;
//     public float oFlightHeight;
//     public int flaps = 4;

//     public float maxSpeed;
//     public float jumpHeight;

//     bool facingRight = true;
//     public float moveDirection = 0;
//     public bool isGrounded = false;
//     public bool onSlope = false;
//     public float slopeMultiplier;
//     Vector3 cameraPos;
//     Rigidbody2D r2d;
//     CapsuleCollider2D mainCollider;
//     Transform t;
//     PlayerMechanics pMe;

//     void Start() {
//         t = transform;
//         r2d = GetComponent<Rigidbody2D>();
//         mainCollider = GetComponent<CapsuleCollider2D>();
//         pMe = GetComponent<PlayerMechanics>();
//         r2d.freezeRotation = true;
//         r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
//         facingRight = t.localScale.x > 0;
//     }

//     void Update() {
//         // Movement controls
//         if (!charging) {
//             if (Input.GetKey(KeyCode.A)) moveDirection = -1;
//             else if (Input.GetKey(KeyCode.D)) moveDirection = 1;
//             else moveDirection = 0;
//         }

//         // Change facing direction
//         if (moveDirection != 0) {
//             if (moveDirection > 0 && !facingRight) {
//                 facingRight = true;
//                 t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
//             }
//             if (moveDirection < 0 && facingRight){
//                 facingRight = false;
//                 t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
//             }
//         }

//         // Jumping
//         if (pMe.character == "owl") {
//             if (Input.GetKeyDown(KeyCode.Space)) {
//                 if (isGrounded) {
//                     maxSpeed = oMaxSpeed;
//                     jumpHeight = oJumpHeight;

//                     r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
//                     flaps = 4;
//                 } else {
//                     maxSpeed = oFlightSpeed;
//                     jumpHeight = oFlightHeight;

//                     if (flaps > 0) {
//                         r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
//                         flaps--;
//                     }
//                 }
//             }

//         } else if (pMe.character == "turtle") {
//             if (Input.GetKeyDown(KeyCode.Space)) {
//                 chargeTime = 0;
//                 moveDirection = 0;
//                 charging = true;
//             }

//             if (Input.GetKey(KeyCode.Space)) {
//                 chargeTime += Time.deltaTime;
//             }

//             if (Input.GetKeyUp(KeyCode.Space)) {
//                 charging = false;

//                 if (facingRight) moveDirection = 1;
//                 else moveDirection = -1;

//                 // if (chargeTime < minChargeTime) maxSlideTime = 6
//                 // else maxSlideTime = 3, 10

//                 sliding = true;
//             }

//             if (sliding) {
//                 if (slideTime < maxSlideTime) {
//                     slideTime += Time.deltaTime;
//                     r2d.velocity = new Vector2((moveDirection) * tSlideSpeed, r2d.velocity.y);
//                 } else {
//                     sliding = false;
//                     slideTime = 0;
//                 }
//             }
//         }
//         // } else {
//         //     if (isGrounded) r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
//         // }
//     }

//     void FixedUpdate() {
//         Bounds colliderBounds = mainCollider.bounds;
//         float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
//         Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        
//         // Check if player is grounded
//         Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        
//         //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
//         // isGrounded = false;
//         // if (colliders.Length > 0) {
//         //     for (int i = 0; i < colliders.Length; i++) {
//         //         if (colliders[i].tag == "Ground") {
//         //             isGrounded = true;
//         //             break;
//         //         }
//         //     }
//         // }

//         // Apply movement velocity
//         if (!sliding) r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
//         if (onSlope) r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y * slopeMultiplier);
//     }

//     void OnCollisionEnter2D(Collision2D collision) {
//         if (collision.gameObject.tag == "Ground")
//             isGrounded = true;
//         if (collision.gameObject.tag == "Slope")
//             onSlope = true;
//     }

//     void OnCollisionExit2D(Collision2D collision) {
//         if (collision.gameObject.tag == "Ground")
//             isGrounded = false;
//         if (collision.gameObject.tag == "Slope")
//             onSlope = false;
//     }
    
// }

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
        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
            moveVelocity = -speed;
            facing = -1;
        }
        if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
            moveVelocity = speed;
            facing = 1;
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

                rb.velocity = new Vector2 (rb.velocity.x, jump);
                if (Random.Range(0,2) == 0) flaps = 1;
                else flaps = 4;

            } else {
                speed = owlSpeedFlight;
                jump = owlJumpFlight;

                if (flaps > 0) {
                    rb.velocity = new Vector2 (rb.velocity.x, jump);
                    flaps--;
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
                sliding = true;

                // 4 ; 0.41 | else 3,10 ; 0.205, 0.67
                if (chargeTime < minChargeTime) 
                    maxSlideTime = 0.41f;
                else maxSlideTime = Random.Range(0.205f, 0.67f);
            }

            if (sliding) {
                if (slideTime < maxSlideTime) {
                    speed = turtleSpeedSlide;
                    moveVelocity = speed * facing;
                    slideTime += Time.deltaTime;
                } else {
                    sliding = false;
                    slideTime = 0;
                    speed = turtleSpeed;
                }
            }
        }
    }

    //Check if Grounded
    // cannot touch 2 at the same time
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
        if (collision.gameObject.tag == "Slope")
            onSlope = true;
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