using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignInWithGoogle()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Configure Google sign-in
        Firebase.Auth.FirebaseUser user = null;
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential("YOUR_GOOGLE_ID_TOKEN", null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
        });
    }
}
