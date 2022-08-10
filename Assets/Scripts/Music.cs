using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
    public AudioSource cave;
    public AudioSource fairy;

    private void OnEnable() {
        SceneController.Level1 += SwitchMusic;
    }

    private void OnDisable() {
        SceneController.Level1 -= SwitchMusic;
    }

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void SwitchMusic() {
        cave.enabled = false;
        fairy.enabled = true;
    }
}