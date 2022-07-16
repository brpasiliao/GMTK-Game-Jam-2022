using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour {
    public int lives;
    PlayerMovement pMo;

    void Start() {
        pMo = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (lives == 0) SceneManager.LoadScene("SampleScene");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            lives--;
            Debug.Log("lives: " + lives);
        }
    }

    
}
