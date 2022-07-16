using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour {
    // public string character;
    public int lives;
    // PlayerMovement pMo;

    void Start() {
        // pMo = GetComponent<PlayerMovement>();
        // changeCharacter("rabbit");
    }

    void Update() {
        if (lives == 0) SceneManager.LoadScene("SampleScene");

        // if (Input.GetKeyDown("1")) changeCharacter("rabbit");
        // if (Input.GetKeyDown("2")) changeCharacter("turtle");
        // if (Input.GetKeyDown("3")) changeCharacter("owl");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            lives--;
            Debug.Log("lives: " + lives);
        }
    }

    // void changeCharacter(string c) {
    //     if (c == "rabbit") {
    //         character = "rabbit";
    //         GetComponent<SpriteRenderer>().color = Color.blue;
    //         // pMo.maxSpeed = pMo.rMaxSpeed;
    //         // pMo.jumpHeight = pMo.rJumpHeight;

    //     } else if (c == "turtle") {
    //         character = "turtle";
    //         GetComponent<SpriteRenderer>().color = Color.green;
    //         // pMo.maxSpeed = pMo.tMaxSpeed;
    //         // pMo.jumpHeight = pMo.tJumpHeight;

    //     } else if (c == "owl") {
    //         character = "owl";
    //         GetComponent<SpriteRenderer>().color = Color.red;
    //         // pMo.maxSpeed = pMo.oMaxSpeed;
    //         // pMo.jumpHeight = pMo.oJumpHeight;
    //     }
    // }
}
