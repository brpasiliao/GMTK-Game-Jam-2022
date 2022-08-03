using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SceneController : MonoBehaviour {
    private void OnEnable() {
        Player.EndLevel += NextScene;
    }

    private void OnDisable() {
        Player.EndLevel -= NextScene;
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextScene() {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial");
    }
}