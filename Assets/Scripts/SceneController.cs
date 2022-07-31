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



}