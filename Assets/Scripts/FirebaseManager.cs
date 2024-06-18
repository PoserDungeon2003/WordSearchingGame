using Firebase.Auth;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async void SignIn() => await SignInWithGoogle();

    private void SignInUsingFirebase(string accessToken)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Configure Google sign-in
        Firebase.Auth.FirebaseUser user = null;
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(null, accessToken);

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

    private async Task SignInWithGoogle()
    {
        ServiceManager.GetService<OpenIDConnectService>().LoginCompleted += OnLoginCompleted;

        await ServiceManager.GetService<OpenIDConnectService>().OpenLoginPageAsync();
    }

    private async void OnLoginCompleted(object sender, EventArgs e)
    {
        Debug.Log("Login completed");
        string accessToken = ServiceManager.GetService<OpenIDConnectService>().AccessToken;
        var userInfo = await ServiceManager.GetService<OpenIDConnectService>().GetUserDataAsync();
        var signupUser = new UserData
        {
            email = userInfo.Email,
            username = userInfo.Username,
        };
        StartCoroutine(ApiClient.Instance.GoogleSignupUser(signupUser));
    }
}
