﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Google;

public class Signin : MonoBehaviour
{
    //higggo@nate.com 12345789

    Firebase.Auth.FirebaseAuth auth;

    private string webClientId = "872497828709-71vq1pf3hud03salf2nquc2b3habhno5.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    public TMPro.TMP_InputField Signin_Email;
    public TMPro.TMP_InputField Signin_Password;

    public TMPro.TMP_InputField Signup_Email;
    public TMPro.TMP_InputField Signup_Password;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };

    }
    public void SigningIn()
    {
        auth.SignInWithEmailAndPasswordAsync(Signin_Email.text, Signin_Password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            ThreadDispatcher.I.RunOnMainThread(() => { NextScene(); return 0; });
        });
    }

    public async void SigningUp()
    {
        await auth.CreateUserWithEmailAndPasswordAsync(Signup_Email.text, Signup_Password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            ThreadDispatcher.I.RunOnMainThread(() => { NextScene(); return 0; });
        });
    }
    public void SigninAnonymous()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            ThreadDispatcher.I.RunOnMainThread(() => { NextScene(); return 0; });
        });
    }

    public void SigninGoogle()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);

    }
    
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            ThreadDispatcher.I.RunOnMainThread(() => { NextScene(); return 0; });
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(1);
    }
}