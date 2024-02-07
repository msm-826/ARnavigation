using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class AuthManager : ScriptableObject {

    // Firebase Authentication instance.
    protected Firebase.Auth.FirebaseAuth auth;

    // Firebase User keyed by Firebase Auth.
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    // Handle initialization of the necessary firebase modules:
    public void InitializeFirebase () {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged (object sender, EventArgs eventArgs) {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;

        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
        if (senderAuth == auth && senderAuth.CurrentUser != user) {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;

            if (!signedIn && user != null) {
                Debug.Log($"Signed out {user.UserId} ");
                SceneManager.LoadScene("SignInScene");
            }

            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;

            if (signedIn) {
                Debug.Log($"Signed in {user.DisplayName}");
                SceneManager.LoadScene("MainMenuScene");
            }
        } else {
            SceneManager.LoadScene("SignInScene");
        }
    }

    // Clean up auth state and auth.
    void OnDestroy () {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}