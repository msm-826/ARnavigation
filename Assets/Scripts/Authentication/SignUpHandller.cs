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
    protected string displayName = "";

    // Start is called before the first frame update
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

    // Create a user with the email and password.
    public Task CreateUserWithEmailAsync() {
        string email = emailField.text;
        string password = passwordField.text;

        Debug.Log($"Attempting to create user: {email}...");
        DisableUIElements();

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread((task) => {
                EnableUIElements();
                LogTaskCompletion(task, "User Creation");
                return task;
            }).Unwrap();
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
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
                    authErrorCode = $"AuthError: {(AuthError) firebaseEx.ErrorCode}";
                    GetErrorMessage((AuthError) firebaseEx.ErrorCode);
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        } else if (task.IsCompleted) {
            Debug.Log(operation + " completed");
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

    /*// Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync (string newDisplayName = null) {
        if (auth.CurrentUser == null) {
            Debug.Log("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        Debug.Log($"Updating user profile: {displayName} ");
        return auth.CurrentUser.UpdateUserProfileAsync(new UserProfile {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        });
    }*/

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
