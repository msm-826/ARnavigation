using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class SignOutListner : MonoBehaviour {

    [SerializeField] private Button signOutButton;

    protected FirebaseAuth auth;

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
        signOutButton.onClick.AddListener(() => auth.SignOut());
    }
}