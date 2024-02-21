using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpHandller : MonoBehaviour {

    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text alertLabel;

    protected FirebaseAuth auth;

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
        signUpButton.onClick.AddListener(() => SignUp());
        backButton.onClick.AddListener(() => SceneManager.LoadScene("SignInScene"));
    }

    private void SignUp() { 
        alertLabel.enabled = false;
        if (passwordField.text != confirmPasswordField.text) {
            alertLabel.text = "Passwords do not match";
            alertLabel.enabled = true;
        } else {
            CreateUserWithEmailAsync();
        }
    }

    public void CreateUserWithEmailAsync () {
        string email = emailField.text;
        string password = passwordField.text;

        Debug.Log($"Attempting to create user: {email}...");
        DisableUIElements();

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => { 
            EnableUIElements();

            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
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
            Debug.LogFormat($"Firebase user created successfully: {result.User.Email} ({result.User.UserId})");
        });
    }

    private void GetErrorMessage (AuthError errorCode) {
        switch (errorCode) {
            case AuthError.MissingPassword:
                alertLabel.text = "Missing password.";
                alertLabel.enabled = true;
                break;
            case AuthError.WeakPassword:
                alertLabel.text = "Too weak of a password.";
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
            case AuthError.EmailAlreadyInUse:
                alertLabel.text = "Email already in use.";
                alertLabel.enabled = true;
                break;
            default:
                alertLabel.text = "Unknown error occurred.";
                alertLabel.enabled = true;
                break;
        }
    }

    private void DisableUIElements() {
        emailField.DeactivateInputField();
        passwordField.DeactivateInputField();
        confirmPasswordField.DeactivateInputField();
        signUpButton.interactable = false;
        backButton.interactable = false;
        alertLabel.enabled = false;
    }

    private void EnableUIElements () {
        emailField.ActivateInputField();
        passwordField.ActivateInputField();
        confirmPasswordField.ActivateInputField();
        signUpButton.interactable = true;
        backButton.interactable = true;
    }
}