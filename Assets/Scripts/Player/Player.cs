using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {
    public delegate void OnEndLevel();
    public static event OnEndLevel EndLevel;

    public Rigidbody2D rb;
    public GameObject poof;

    protected float moveVelocity;
    protected float currentSpeed;
    protected float currentJump;

    private Vector2 lastGrounded;
    protected bool isGrounded = true;

    protected int facing = 1;
    protected bool isHit = false;
    private bool isSafe = false;
    public float safeTime = 1f;
    private float safeTimer = 0f;

    private void OnEnable() {
        Narration.Narrate += ChangeCharacter;
    }

    private void OnDisable() {
        Narration.Narrate -= ChangeCharacter;
    }

    private void Start() {
        rb.gravityScale = 10;
        lastGrounded = transform.position;
    }

    private void Update () {
        if (isGrounded && rb.velocity.y == 0 && rb.velocity.x == 0) 
            lastGrounded = transform.position;

        moveVelocity = 0;
        Special();
        Move();
        rb.velocity = new Vector2(moveVelocity, rb.velocity.y);

        PlayAnimations();
        CheckSafe();

        DevSecret();
    }

    protected virtual void Move() {
        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            facing = -1;
            moveVelocity = -currentSpeed;
        }
        if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            facing = 1;
            moveVelocity = currentSpeed;
        }

        if (isHit) moveVelocity = -5 * facing;
    }

    protected virtual void Special() {}





    // cannot touch 2 at the same time
    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
            isHit = false;
        }

        if (collision.gameObject.tag == "Enemy")
            EnemyContact(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Fell") {
            transform.position = lastGrounded;
            rb.velocity = new Vector2 (0, 0);
            ResetValues();
        }

        if (collider.gameObject.name == "EndLevel") {
            rb.gravityScale = 0;
            rb.velocity = new Vector2 (rb.velocity.x, -3);
        }
        if (collider.gameObject.name == "NextLevel") {
            EndLevel?.Invoke();
        }
    }

    protected virtual void EnemyContact(GameObject enemy) {}

    protected virtual void ResetValues() {}

    protected void GetHurt(GameObject enemy) {
        Debug.Log("got hurt");
        isSafe = true;

        if (!isHit) {
            rb.velocity = new Vector2(rb.velocity.x, 20);
            ResetValues();
            isHit = true;

            // ChangeCharacter(enemy.GetComponent<Enemy>().form);
        }
    }

    void CheckSafe() {
        if (isSafe) {
            if (safeTimer < safeTime) 
                safeTimer += Time.deltaTime;
            else {
                safeTimer = 0f;
                isSafe = false;
            }
        }
    }

    void DevSecret() {
        if (Input.GetKeyDown("1")) ChangeCharacter("rabbit");
        if (Input.GetKeyDown("2")) ChangeCharacter("turtle");
        if (Input.GetKeyDown("3")) ChangeCharacter("owl");
    }

    void ChangeCharacter(string c) {
        ResetValues();

        GetComponent<Rabbit>().enabled = false;
        GetComponent<Turtle>().enabled = false;
        GetComponent<Owl>().enabled = false;

        if (c == "rabbit") GetComponent<Rabbit>().enabled = true;
        else if (c == "turtle") GetComponent<Turtle>().enabled = true;
        else if (c == "owl") GetComponent<Owl>().enabled = true;

        StartCoroutine("PlayPoof");
    }

    void PlayAnimations() {
        // if (shell.activeSelf && Input.GetKeyDown(KeyCode.Space) && !(shell.GetComponent<Turtle>().isDashing || shell.GetComponent<Turtle>().isSliding))
        //     GetComponent<Animator>().Play("Shell_Into");
        // else if (shell.activeSelf && shell.GetComponent<Turtle>().isCharging)
        //     GetComponent<Animator>().Play("Shell_Rev");
        // else if (shell.activeSelf && shell.GetComponent<Turtle>().isDashing)
        //     GetComponent<Animator>().Play("Shell_Spin_Start");
        // else if (shell.activeSelf && shell.GetComponent<Turtle>().isSliding)
        //     GetComponent<Animator>().Play("Shell_Spin_End");
        // else if (isGrounded && (rb.velocity.x > 0.001f || rb.velocity.x < -0.001f))
        //     GetComponent<Animator>().Play("Run");
        // else if (!isGrounded && rb.velocity.y > 0)
        //     GetComponent<Animator>().Play("Jump_Up");
        // else if (!isGrounded && rb.velocity.y < -0.001f) 
        //     GetComponent<Animator>().Play("Jump_Down");
        // else GetComponent<Animator>().Play("Idle");
    }

    IEnumerator PlayPoof() {
        poof.SetActive(true);
        poof.GetComponent<Animator>().Play("Poof", -1, 0f);
        yield return new WaitForSeconds(poof.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
        poof.SetActive(false);
    }
}