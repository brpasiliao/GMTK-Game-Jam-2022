using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class PlayerMovement : MonoBehaviour {
    // Move player in 2D space
    public float rMaxSpeed;
    public float rJumpHeight;

    public float tMaxSpeed;
    public float tJumpHeight;

    public float oMaxSpeed;
    public float oJumpHeight;

    public float maxSpeed;
    public float jumpHeight;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CircleCollider2D mainCollider;
    Transform t;
    PlayerMechanics pMe;

    void Start() {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CircleCollider2D>();
        pMe = GetComponent<PlayerMechanics>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        facingRight = t.localScale.x > 0;
    }

    void Update() {
        // Movement controls
        if (Input.GetKey(KeyCode.A)) moveDirection = -1;
        else if (Input.GetKey(KeyCode.D)) moveDirection = 1;
        else moveDirection = 0;

        // Change facing direction
        if (moveDirection != 0) {
            if (moveDirection > 0 && !facingRight) {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight){
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if (pMe.character == "owl") {
            if (Input.GetKeyDown(KeyCode.W))
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        } else {
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }
    }

    void FixedUpdate() {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(0.5f, colliderRadius * 0.9f, 0);
        
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0) {
            for (int i = 0; i < colliders.Length; i++){
                if (colliders[i].tag == "Ground") {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2((moveDirection) * maxSpeed, r2d.velocity.y);
    }
}