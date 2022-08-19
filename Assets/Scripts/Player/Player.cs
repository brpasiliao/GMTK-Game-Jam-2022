using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {
    public delegate void OnEndLevel();
    public static event OnEndLevel EndLevel;

    public Rigidbody2D rb;
    public GameObject wings;
    public GameObject shell;
    public GameObject ears;
    public GameObject poof;

    public Vector2 lastGrounded;
    public bool isGrounded = true;

    public int facing = 1;
    public bool isHit = false;
    public bool isSafe = false;
    public float safeTime = 1f;
    float safeTimer = 0f;

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
        if (isGrounded && rb.velocity.y == 0 && rb.velocity.x == 0) lastGrounded = transform.position;

        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            facing = -1;
        }
        else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            facing = 1;
        }

        PlayAnimations();
        CheckSafe();

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
            shell.GetComponent<Turtle>().direction = -shell.GetComponent<Turtle>().direction;

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
        }

        if (collider.gameObject.name == "EndLevel") {
            rb.gravityScale = 0;
            rb.velocity = new Vector2 (rb.velocity.x, -3);
        }
        if (collider.gameObject.name == "NextLevel") {
            EndLevel?.Invoke();
        }
    }

    void EnemyContact(GameObject enemy) {
        // affecting enemy object
        if ((ears.activeSelf && rb.velocity.y < 0f) ||
            (shell.activeSelf && shell.GetComponent<Turtle>().isDashing) ||
            (wings.activeSelf && wings.GetComponent<Owl>().flaps > 0 && Input.GetKeyDown(KeyCode.Space)))
            enemy.SetActive(false);

        else if (!isSafe) {
            isSafe = true;

            if (!isHit) {
                rb.velocity = new Vector2(rb.velocity.x, 20);
                isHit = true;

                ChangeCharacter(enemy.GetComponent<Enemy>().form);
            }
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
        if (c == "rabbit" && !ears.activeSelf) {
            wings.SetActive(false);
            shell.SetActive(false);
            ears.SetActive(true);
            ears.GetComponent<SpriteRenderer>().enabled = true;
        } else if (c == "turtle" && !shell.activeSelf) {
            wings.SetActive(false);
            shell.SetActive(true);
            ears.SetActive(false);
            shell.GetComponent<SpriteRenderer>().enabled = true;
        } else if (c == "owl" && !wings.activeSelf) {
            wings.SetActive(true);
            shell.SetActive(false);
            ears.SetActive(false);
            wings.GetComponent<SpriteRenderer>().enabled = true;
        }

        StartCoroutine("PlayPoof");
    }

    void PlayAnimations() {
        if (shell.activeSelf && Input.GetKeyDown(KeyCode.Space) && !(shell.GetComponent<Turtle>().isDashing || shell.GetComponent<Turtle>().isSliding))
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

    IEnumerator PlayPoof() {
        poof.SetActive(true);
        poof.GetComponent<Animator>().Play("Poof", -1, 0f);
        yield return new WaitForSeconds(poof.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
        poof.SetActive(false);
    }
}