using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneHandler : MonoBehaviour {

    [SerializeField] private Button getStartedButton;
    [SerializeField] private Button howToButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text loaggedInEmail;

    protected FirebaseAuth auth;

    void Start() {
        getStartedButton.onClick.AddListener(() => SceneManager.LoadScene("MarkerDetectionScene"));
        howToButton.onClick.AddListener(() => SceneManager.LoadScene("HowToScene"));
        exitButton.onClick.AddListener(() => Application.Quit());

        auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        loaggedInEmail.text = $"Logged in using: {user.Email}";
        Debug.Log($"Display name: {user.DisplayName}");
        Debug.Log($"Email: {user.Email}");
        Debug.Log($"Provider id: {user.ProviderId}");
        Debug.Log($"Provider id: {user.ProviderData}");
        Debug.Log($"Photo url: {user.PhotoUrl}");
    }

    private void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
