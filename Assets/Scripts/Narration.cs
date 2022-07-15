using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narration : MonoBehaviour {
    public Text textBox;
    public float slowSpeed;
    public float fastSpeed;
    private bool forward = false;

    void Start() {
        // StartCoroutine("DisplayMessage", "ffknslkn efskfn esfjnes eslfnseflke fsefnsfekfnrs slf slfjnf slfj elfk sf sfljsnfj ffknslkn efskfn esfjnes eslfnseflke fsefnsfekfnrs slf slfjnf slfj elfk sf sfljsnfj ffknslkn efskfn esfjnes eslfnseflke fsefnsfekfnrs slf slfjnf slfj elfk sf sfljsnfj");
    }

    void Update() {
        if (Input.GetKeyDown("space")) forward = true;
    }

    IEnumerator DisplayMessage(string text) {
        textBox.text = "";

        foreach (char c in text) {
            textBox.text = textBox.text + c;
            Canvas.ForceUpdateCanvases();
            
            if (forward) yield return new WaitForSeconds(fastSpeed);
            else yield return new WaitForSeconds(slowSpeed);
        }

        forward = false;
    }
}

// moving, action button


// 3 lives
// object to lose life

// how to share
// ui?