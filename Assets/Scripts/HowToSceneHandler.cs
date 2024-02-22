using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToSceneHandler : MonoBehaviour {

    [SerializeField] private Button backButton;
    
    private void Start() {
        backButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenuScene"));
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
