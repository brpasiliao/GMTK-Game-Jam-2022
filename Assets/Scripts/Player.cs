using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour {
    // public delegate void OnGameOver();
    // public static event OnGameOver GameOver;

    public Rigidbody2D rb;
    public GameObject wings;
    public GameObject shell;
    public GameObject ears;
    public GameObject poof;
    private Component currentForm;

    public Vector2 lastGrounded;
    public bool isGrounded = true;

    public bool isHit = false;
    public bool isSafe = false;
    public float safeTime = 1f;
    float safeTimer = 0f;

    // public float endVelocity;

    private void OnEnable() {
        Narration.Narrate += ChangeCharacter;
    }

    private void OnDisable() {
        Narration.Narrate -= ChangeCharacter;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 10;
        lastGrounded = transform.position;
    }

    void Update () {
        if (isGrounded && rb.velocity.y == 0) lastGrounded = transform.position;
        CheckSafe();
        PlayAnimations();

        DevSecret();
    }

    // cannot touch 2 at the same time
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
            isHit = false;
        }
        if (collision.gameObject.tag == "Wall" && shell.activeSelf && 
            (shell.GetComponent<Turtle>().isDashing || shell.GetComponent<Turtle>().isSliding))
            shell.GetComponent<Turtle>().facing = -shell.GetComponent<Turtle>().facing;

        // if (!isSafe && collision.gameObject.tag == "Enemy")
        if (collision.gameObject.tag == "Enemy")
            GetHit(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Fell") {
            transform.position = lastGrounded;
        }

        // if (collider.gameObject.name == "EndLevel") {
        //     rb.gravityScale = 0;
        //     rb.velocity = new Vector2 (rb.velocity.x, endVelocity);
        // }
        // if (collider.gameObject.name == "NextLevel" && SceneManager.GetActiveScene().name == "Tutorial") {
        //     ChangeScene("Level 1");
        // }
        // if (collider.gameObject.name == "NextLevel" && SceneManager.GetActiveScene().name == "Level 1") {
        //     ChangeScene("End");
        // }
    }

    void GetHit(GameObject enemy) {
        // knockback
        // currentForm.Knockback();
        isSafe = true;
        isHit = true;
        rb.velocity = new Vector2(rb.velocity.x, 20);

        Debug.Log("ouch");
        // change character
        // ChangeCharacter(enemy.GetComponent<Enemy>().form);
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
        if (c == "rabbit") {
            wings.SetActive(false);
            shell.SetActive(false);
            ears.SetActive(true);
            ears.GetComponent<SpriteRenderer>().enabled = true;
            currentForm = ears.GetComponent<Rabbit>();
        } else if (c == "turtle") {
            wings.SetActive(false);
            shell.SetActive(true);
            ears.SetActive(false);
            shell.GetComponent<SpriteRenderer>().enabled = true;
            currentForm = shell.GetComponent<Turtle>();
        } else if (c == "owl") {
            wings.SetActive(true);
            shell.SetActive(false);
            ears.SetActive(false);
            wings.GetComponent<SpriteRenderer>().enabled = true;
            currentForm = wings.GetComponent<Owl>();
        }

        // poof.SetActive(true);
        // poof.GetComponent<Animator>().Play("Poof");
        // poof.SetActive(false);
    }

    void PlayAnimations() {
        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        if (shell.activeSelf && Input.GetKeyDown(KeyCode.Space))
            GetComponent<Animator>().Play("Shell_Into");
        else if (shell.activeSelf && shell.GetComponent<Turtle>().isCharging)
            GetComponent<Animator>().Play("Shell_Rev");
        else if (shell.activeSelf && shell.GetComponent<Turtle>().isDashing)
            GetComponent<Animator>().Play("Shell_Spin_Start");
        else if (shell.activeSelf && shell.GetComponent<Turtle>().isSliding)
            GetComponent<Animator>().Play("Shell_Spin_End");
        else if (isGrounded && (rb.velocity.x > 0.001f || rb.velocity.x < -0.001f))
            GetComponent<Animator>().Play("Run");
        else if (!isGrounded && rb.velocity.y > 0)
            GetComponent<Animator>().Play("Jump_Up");
        else if (!isGrounded && rb.velocity.y < -0.001f) 
            GetComponent<Animator>().Play("Jump_Down");
        else GetComponent<Animator>().Play("Idle");
    }
}