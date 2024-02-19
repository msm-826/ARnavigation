using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToSceneHandler : MonoBehaviour {

    [SerializeField] private Button backButton;
    
    void Start() {
        backButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenuScene"));
    }
}
