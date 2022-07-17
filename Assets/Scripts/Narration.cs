using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narration : MonoBehaviour {
    TMPro.TextMeshProUGUI textBox;
    public PlayerMovement pm;

    public string dialogue;
    public float speed;
    public string character = "";
    bool done;
    // public float slowSpeed;
    // public float fastSpeed;
    // private bool forward = false;

    void Start() {
        textBox = GetComponent<TMPro.TextMeshProUGUI>();
        dialogue = textBox.text;
        textBox.text = "";
    }

    void Update() {}

    void OnTriggerEnter2D(Collider2D collider) {
        if (!done && collider.gameObject.name == "Player") {
            StartCoroutine("DisplayMessage", dialogue);
            done = true;
        }
        if (character != "") {
            pm.ChangeCharacter(character);
        }
    }

    IEnumerator DisplayMessage(string t) {
        // textBox.text = "";


        foreach (char c in t) {
            textBox.text = textBox.text + c;
            Canvas.ForceUpdateCanvases();
            
            // if (forward) yield return new WaitForSeconds(fastSpeed);
            // else yield return new WaitForSeconds(slowSpeed);
            yield return new WaitForSeconds(speed);
        }

        // forward = false;
    }
}



// start, 
// tutorial, (make collider at pit: next scene, on level complete dialogue)
// black screen
// level 1, 
// end

// death = change character
// music