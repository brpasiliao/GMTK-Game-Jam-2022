using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour {
    public int lives;

    PlayerMovement pMo;
    Rigidbody2D rb;

    public float safeTime = 1f;
    float safeTimer = 0f;
    bool safe = false;

    public float endVelocity;

    void Start() {
        pMo = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (lives == 0) ChangeScene(SceneManager.GetActiveScene().name);

        if (safe) {
            if (safeTimer < safeTime) 
                safeTimer += Time.deltaTime;
            else {
                safeTimer = 0f;
                safe = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!safe && collision.gameObject.tag == "Enemy") {
            if (pMo.character == "turtle" && pMo.dashing) {
                collision.gameObject.SetActive(false);
            } else {
                lives--;
                safe = true;
                Debug.Log("lives: " + lives);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Fell") {
            transform.position = pMo.lastGrounded; // + new Vector2(0, 2);
        }

        if (collider.gameObject.name == "EndLevel") {
            rb.gravityScale = 0;
            rb.velocity = new Vector2 (rb.velocity.x, endVelocity);
        }
        if (collider.gameObject.name == "NextLevel") {
            ChangeScene("Level 1");
        }
    }

    public void ChangeScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}
