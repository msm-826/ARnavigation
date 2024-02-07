using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firebase.Extensions;
using System;

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

    public Task SignInWithEmailAsync () {
        var email = emailField.text;
        var password = passwordField.text;

        Debug.Log($"Attempting to sign in as: {email}...");
        DisableUIElements();

        return auth.SignInWithEmailAndPasswordAsync(email, password)
              .ContinueWithOnMainThread(task => {
                  EnableUIElements();
                  if (LogTaskCompletion(task, "Sign-in")) {
                      Debug.Log($"{task.Result.User.DisplayName} signed in");
                  }
              });
    }

    protected bool LogTaskCompletion (Task task, string operation) {
        bool complete = false;
        if (task.IsCanceled) {
            Debug.Log($"{operation} canceled.");
        } else if (task.IsFaulted) {
            Debug.Log($"{operation} encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null) {
                    authErrorCode = $"AuthError: {((AuthError) firebaseEx.ErrorCode)}: ";
                    GetErrorMessage((AuthError) firebaseEx.ErrorCode);
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        } else if (task.IsCompleted) {
            Debug.Log($"{operation} completed");
            complete = true;
        }
        return complete;
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