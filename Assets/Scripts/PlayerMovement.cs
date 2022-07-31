using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public Vector2 lastGrounded;
    public bool isGrounded = true;

    public Rigidbody2D rb;

    public GameObject wings;
    public GameObject shell;
    public GameObject ears;
    public GameObject poof;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 10;
        lastGrounded = transform.position;

        ChangeCharacter("rabbit");
        // if (SceneManager.GetActiveScene().name == "Tutorial")
        //     ears.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update () {
        if (isGrounded && rb.velocity.y == 0) lastGrounded = transform.position;

        if (Input.GetKeyDown("1")) ChangeCharacter("rabbit");
        if (Input.GetKeyDown("2")) ChangeCharacter("turtle");
        if (Input.GetKeyDown("3")) ChangeCharacter("owl");

        PlayAnimations();
    }

    // Check if Grounded
    // cannot touch 2 at the same time
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
        // if (collision.gameObject.tag == "Wall" &&
        //     character == "turtle" && (dashing || sliding)) {
        //         facing = -facing;
        //     }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    public void ChangeCharacter(string c) {
        if (c == "rabbit") {
            wings.SetActive(false);
            shell.SetActive(false);
            ears.SetActive(true);
        } else if (c == "turtle") {
            wings.SetActive(false);
            shell.SetActive(true);
            ears.SetActive(false);

        } else if (c == "owl") {
            wings.SetActive(true);
            shell.SetActive(false);
            ears.SetActive(false);
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
        else if (shell.activeSelf && shell.GetComponent<Turtle>().charging)
            GetComponent<Animator>().Play("Shell_Rev");
        else if (shell.activeSelf && shell.GetComponent<Turtle>().dashing)
            GetComponent<Animator>().Play("Shell_Spin_Start");
        else if (shell.activeSelf && shell.GetComponent<Turtle>().sliding)
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