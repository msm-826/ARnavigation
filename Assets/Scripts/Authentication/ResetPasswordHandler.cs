using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetPasswordHandler : MonoBehaviour {

    [SerializeField] private Button resetPasswordButton;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_Text alertLabel;

    protected FirebaseAuth auth;

    private void Start() {
        auth = FirebaseAuth.DefaultInstance;
        resetPasswordButton.onClick.AddListener(() => ResetPasswordAsync());
    }

    private void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene("SignInScene");
        }
    }

    public void ResetPasswordAsync() {
        var emailAddress = emailField.text;

        auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
            if (task.IsCanceled) {
                alertLabel.text = "Operation was cancelled.";
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            
            if (task.IsFaulted) {
                alertLabel.text = "Operation ended in an error.";
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            alertLabel.text = "Password reset email sent successfully";
        });
    }
}
