using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Control : MonoBehaviour {

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial");
    }

}