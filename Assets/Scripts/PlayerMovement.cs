using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public string character;
    
    public float rMaxSpeed;
    public float rJumpHeight;

    public float tMaxSpeed;
    public float tSlideSpeed;
    public float tJumpHeight;
    public float minChargeTime;
    public float chargeTime = 0;
    public bool charging = false;
    public float slideTime = 0;
    public float maxSlideTime;
    public bool sliding = false;

    public float oMaxSpeed;
    public float oFlightSpeed;
    public float oJumpHeight;
    public float oFlightHeight;
    public int flaps = 4;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private int facingDirection = 1;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private void Start() {
        ChangeCharacter("rabbit");

        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        capsuleColliderSize = cc.size;
    }

    private void Update() {
        if (Input.GetKeyDown("1")) ChangeCharacter("rabbit");
        if (Input.GetKeyDown("2")) ChangeCharacter("turtle");
        if (Input.GetKeyDown("3")) ChangeCharacter("owl");

        CheckInput();     
    }

    private void FixedUpdate() {
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    private void CheckInput() {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.A) && facingDirection == -1) Flip();
        else if (Input.GetKeyDown(KeyCode.D) && facingDirection == 1) Flip();

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }
    
    private void CheckGround() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.velocity.y <= 0.0f)
            isJumping = false;

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
            canJump = true;
    }

    private void SlopeCheck() {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos) {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront) {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack) {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos) {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit) {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
                isOnSlope = true;

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
            canWalkOnSlope = false;
        else
            canWalkOnSlope = true;

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
            rb.sharedMaterial = fullFriction;
        else
            rb.sharedMaterial = noFriction;
    }

    private void Jump() {
        if (canJump) {
            canJump = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
            newForce.Set(0.0f, jumpForce);
            rb.AddForce(newForce, ForceMode2D.Impulse);
        }

        // if (pMe.character == "owl") {
        //     if (isGrounded) {
        //         maxSpeed = oMaxSpeed;
        //         jumpHeight = oJumpHeight;

        //         r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        //         flaps = 4;
        //     } else {
        //         maxSpeed = oFlightSpeed;
        //         jumpHeight = oFlightHeight;

        //         if (flaps > 0) {
        //             r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        //             flaps--;
        //         }
        //     }
        // }

        // } else if (pMe.character == "turtle") {
        //     if (Input.GetKeyDown(KeyCode.Space)) {
        //         chargeTime = 0;
        //         moveDirection = 0;
        //         charging = true;
        //     }

        //     if (Input.GetKey(KeyCode.Space)) {
        //         chargeTime += Time.deltaTime;
        //     }

        //     if (Input.GetKeyUp(KeyCode.Space)) {
        //         charging = false;

        //         if (facingRight) moveDirection = 1;
        //         else moveDirection = -1;

        //         // if (chargeTime < minChargeTime) maxSlideTime = 6
        //         // else maxSlideTime = 3, 10

        //         sliding = true;
        //     }

        //     if (sliding) {
        //         if (slideTime < maxSlideTime) {
        //             slideTime += Time.deltaTime;
        //             r2d.velocity = new Vector2((moveDirection) * tSlideSpeed, r2d.velocity.y);
        //         } else {
        //             sliding = false;
        //             slideTime = 0;
        //         }
        //     }
        // }
    }   

    private void ApplyMovement() {
        if (isGrounded && !isOnSlope && !isJumping) { //if not on slope
            Debug.Log("This one");
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;
        }
        else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) { //If on slope
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            rb.velocity = newVelocity;
        }
        else if (!isGrounded) { //If in air
            newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }

    private void Flip() {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void ChangeCharacter(string c) {
        if (c == "rabbit") {
            character = "rabbit";
            GetComponent<SpriteRenderer>().color = Color.blue;
            // pMo.maxSpeed = pMo.rMaxSpeed;
            // pMo.jumpHeight = pMo.rJumpHeight;

        } else if (c == "turtle") {
            character = "turtle";
            GetComponent<SpriteRenderer>().color = Color.green;
            // pMo.maxSpeed = pMo.tMaxSpeed;
            // pMo.jumpHeight = pMo.tJumpHeight;

        } else if (c == "owl") {
            character = "owl";
            GetComponent<SpriteRenderer>().color = Color.red;
            // pMo.maxSpeed = pMo.oMaxSpeed;
            // pMo.jumpHeight = pMo.oJumpHeight;
        }
    }

}
