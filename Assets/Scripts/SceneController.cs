using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SceneController : MonoBehaviour {

    // private void OnEnable() {
    //     PlayerStats.GameOver += RestartScene;
    // }

    // private void OnDisable() {
    //     PlayerStats.GameOver -= RestartScene;
    // }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

    // if (collider.gameObject.name == "EndLevel") {
        //     rb.gravityScale = 0;
        //     rb.velocity = new Vector2 (rb.velocity.x, endVelocity);
        // }
        // if (collider.gameObject.name == "NextLevel" && SceneManager.GetActiveScene().name == "Tutorial") {
        //     ChangeScene("Level 1");
        // }
        // if (collider.gameObject.name == "NextLevel" && SceneManager.GetActiveScene().name == "Level 1") {
        //     ChangeScene("End");
        // }

}