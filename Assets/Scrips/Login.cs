using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System;


public class Login : MonoBehaviour
{
    [Header("Login")]
    public GameObject panelLogin;
    public InputField userL;
    public InputField passL;
    public Text warningLogLoginText;
    public Text confirmLoginText;

    [Header("Register")]
    public GameObject panelRegistro;
    public InputField nameR;
    public InputField emailR;
    public InputField passR1;
    public InputField passR2;
    public Text warningRegisterText;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    private void Awake()
    {
        //This already happens in the background. When you open the application you need to check if auth.CurrentUser !=null and if so just skip the login screen.

        //check all necessary dependecies 
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Please, resolve all Firebase dependecies: " + dependencyStatus);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeFirebase()
    {
        //verificar 
        Debug.Log("Setting Firebase");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton()
    {
        StartCoroutine(Logiin(userL.text, passL.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Regiister(emailR.text, passR1.text, nameR.text));
    }

    private IEnumerator Logiin(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid email";
                    break;
                case AuthError.UserNotFound:
                    message = "User not fund";
                    break;
            }
            warningLogLoginText.text = message;
        }
        else
        {
            user = LoginTask.Result;
            Debug.LogFormat("User in succefully: {0} ({1}) ", user.DisplayName, user.Email);
            warningLogLoginText.text = "";
            confirmLoginText.text = "Logged In";
            //cargar la siguiente escena
        }
    }

    private IEnumerator Regiister(string _email, string _pass, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passR1.text != passR2.text)
        {
            warningRegisterText.text = "Password doesnt match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _pass);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email already in use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                user = RegisterTask.Result;

                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    var ProfileTask = user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                        FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "User name set failed";
                    }
                    else
                    {
                        ToLogin();
                        warningRegisterText.text = "";
                    }
                }
            }



        }
    } 

    public void ToRegistro()
    {
        userL.text = "";
        passL.text = "";
        warningLogLoginText.text = "";

        panelLogin.SetActive(false);
        panelRegistro.SetActive(true);
    }

    public void ToLogin()
    {
        nameR.text = "";
        emailR.text = "";
        passR1.text = "";
        passR2.text = "";
        warningRegisterText.text = "";

        panelRegistro.SetActive(false);
        panelLogin.SetActive(true);
    }

}
