using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narration : MonoBehaviour {
    public delegate void OnNarrate(string character);
    public static event OnNarrate Narrate;

    TMPro.TextMeshProUGUI textBox;
    // public PlayerMovement pm;

    public string dialogue;
    public float speed;
    public string character = "";
    bool done;

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
            // pm.ChangeCharacter(character);
            Narrate?.Invoke(character);
        }
    }

    IEnumerator DisplayMessage(string t) {
        foreach (char c in t) {
            textBox.text = textBox.text + c;
            Canvas.ForceUpdateCanvases();
            
            yield return new WaitForSeconds(speed);
        }
    }
}



// start, 
// tutorial, (make collider at pit: next scene, on level complete dialogue)
// black screen
// level 1, 
// end

// death = change character
// music