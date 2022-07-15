using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMechanics : MonoBehaviour
{
    public int lives;

    void Start() {}

    void Update() {
        if (lives == 0) SceneManager.LoadScene("SampleScene");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("hi");
        if (collision.gameObject.tag == "Enemy") {
            lives--;
        }
    }
}
