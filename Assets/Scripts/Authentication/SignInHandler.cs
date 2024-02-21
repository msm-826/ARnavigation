using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using Firebase;

public class SignInHandler :MonoBehaviour {

    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private TMP_Text alertLabel;

    protected FirebaseAuth auth;

    void Start () {
        auth = FirebaseAuth.DefaultInstance;
        signInButton.onClick.AddListener(() => SignInWithEmailAsync());
        signUpButton.onClick.AddListener(() => SceneManager.LoadScene("SignUpScene"));
    }

    public void SignInWithEmailAsync () {
        var email = emailField.text;
        var password = passwordField.text;

        Debug.Log($"Attempting to sign in as: {email}...");
        DisableUIElements();

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            EnableUIElements();

            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;

            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception.Message);
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null) {
                        Debug.LogError($"AuthError: {((AuthError) firebaseEx.ErrorCode)}");
                        GetErrorMessage((AuthError) firebaseEx.ErrorCode);
                    }
                }
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.Email} ({result.User.UserId})");
        });
    }

    private void GetErrorMessage (AuthError errorCode) {
        switch (errorCode) {
            case AuthError.MissingPassword:
                alertLabel.text = "Missing password.";
                alertLabel.enabled = true;
                break;
            case AuthError.WrongPassword:
                alertLabel.text = "Incorrect password.";
                alertLabel.enabled = true;
                break;
            case AuthError.InvalidEmail:
                alertLabel.text = "Invalid email.";
                alertLabel.enabled = true;
                break;
            case AuthError.MissingEmail:
                alertLabel.text = "Missing email.";
                alertLabel.enabled = true;
                break;
            case AuthError.UserNotFound:
                alertLabel.text = "Account not found.";
                alertLabel.enabled = true;
                break;
            default:
                alertLabel.text = "Unknown error occurred.";
                alertLabel.enabled = true;
                break;
        }
    }

    private void DisableUIElements () {
        emailField.DeactivateInputField();
        passwordField.DeactivateInputField();
        signInButton.interactable = false;
        signUpButton.interactable = false;
        alertLabel.enabled = false;
    }

    private void EnableUIElements () {
        emailField.ActivateInputField();
        passwordField.ActivateInputField();
        signInButton.interactable = true;
        signUpButton.interactable = true;
    }
}