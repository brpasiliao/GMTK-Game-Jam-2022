using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SceneController : MonoBehaviour {
    public delegate void OnLevel1();
    public static event OnLevel1 Level1;

    private void OnEnable() {
        Player.EndLevel += NextScene;
    }

    private void OnDisable() {
        Player.EndLevel -= NextScene;
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == "Level 1") {
            Debug.Log("level 1");
            Level1?.Invoke();
        }
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial");
    }
}