using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public string character;
    public Vector2 lastGrounded;

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

    public GameObject body;
    public GameObject wings;
    public GameObject shell;
    public GameObject ears;
    public GameObject poof;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 10;
        lastGrounded = transform.position;

        ChangeCharacter("rabbit");
        if (SceneManager.GetActiveScene().name == "Tutorial")
            ears.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update () {
        moveVelocity = 0;
        if (isGrounded && rb.velocity.y == 0) lastGrounded = transform.position;

        Special();

        //Left Right Movement
        if (character == "turtle" && (dashing || sliding)) {
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

        } else {
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                moveVelocity = -speed;
                facing = -1;
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                moveVelocity = speed;
                facing = 1;
                // transform.Rotate(0, 0, 0);
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }

        if (onSlope) rb.velocity = new Vector2 (moveVelocity, rb.velocity.y * slopeMultiplier);
        else rb.velocity = new Vector2 (moveVelocity, rb.velocity.y);

        if (Input.GetKeyDown("1")) ChangeCharacter("rabbit");
        if (Input.GetKeyDown("2")) ChangeCharacter("turtle");
        if (Input.GetKeyDown("3")) ChangeCharacter("owl");

        PlayAnimations();
    }

    void Special() {
        if (character == "rabbit" && isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                charging = true;
                chargeTime = 0;
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTime += Time.deltaTime;
                if (chargeTime > minChargeTime)
                    speed = 0;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                charging = false;

                if (chargeTime > minChargeTime) {
                    jump = Random.Range(21f, 38f);
                    ears.GetComponent<AudioSource>().Play();
                }

                speed = rabbitSpeed;
                GetComponent<Animator>().Play("Jump_Up");
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
                if (Random.Range(0,2) == 0) flaps = 1;
                else flaps = 4;
                // flaps = 4;

            } else {
                speed = owlSpeedFlight;
                jump = owlJumpFlight;

                if (flaps > 0) {
                    rb.velocity = new Vector2 (rb.velocity.x, jump);
                    flaps--;

                    wings.GetComponent<Animator>().Play("Wing_Flap");
                    wings.GetComponent<AudioSource>().Play();

                } else {
                    wings.GetComponent<Animator>().Play("Wing_Idle");
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
                        GetComponent<Animator>().Play("Shell_Out");
                        shell.GetComponent<SpriteRenderer>().enabled = true;
                        coolDownTimer = 0f;
                        sliding = false;
                        speed = turtleSpeed;
                        slowDown = 0.1f;
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
            }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
        if (collision.gameObject.tag == "Slope")
            onSlope = false;
    }

    public void ChangeCharacter(string c) {
        if (c == "rabbit") {
            character = "rabbit";
            speed = rabbitSpeed;
            jump = rabbitJump;

            wings.SetActive(false);
            shell.SetActive(false);
            ears.SetActive(true);
            ears.GetComponent<SpriteRenderer>().enabled = true;

        } else if (c == "turtle") {
            character = "turtle";
            speed = turtleSpeed;
            jump = turtleJump;

            wings.SetActive(false);
            shell.SetActive(true);
            ears.SetActive(false);
            shell.GetComponent<SpriteRenderer>().enabled = true;

        } else if (c == "owl") {
            character = "owl";
            speed = owlSpeed;
            jump = owlJump;

            wings.SetActive(true);
            shell.SetActive(false);
            ears.SetActive(false);
            wings.GetComponent<SpriteRenderer>().enabled = true;
        }

        poof.SetActive(true);
        poof.GetComponent<Animator>().Play("Poof");
        poof.SetActive(false);
    }

    void PlayAnimations() {
        if (character == "turtle" && Input.GetKeyDown(KeyCode.Space)) {
            shell.GetComponent<SpriteRenderer>().enabled = false;
            shell.GetComponent<AudioSource>().Play();
            GetComponent<Animator>().Play("Shell_Into");
        }
        else if (character == "turtle" && charging)
            GetComponent<Animator>().Play("Shell_Rev");
        else if (character == "turtle" && dashing) {
            shell.GetComponent<AudioSource>().Stop();
            GetComponent<Animator>().Play("Shell_Spin_Start");
        }
        else if (character == "turtle" && sliding)
            GetComponent<Animator>().Play("Shell_Spin_End");
        else if (isGrounded && rb.velocity.x != 0) 
            GetComponent<Animator>().Play("Run");
        else if (!isGrounded && rb.velocity.y > 0) 
            GetComponent<Animator>().Play("Jump_Up");
        else if (!isGrounded && rb.velocity.y <= 0) 
            GetComponent<Animator>().Play("Jump_Down");
        else GetComponent<Animator>().Play("Idle");
    }
}