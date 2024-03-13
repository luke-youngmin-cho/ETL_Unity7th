using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DiceGame.Data;
using Firebase.Auth;
using DiceGame.Data.Firebase;
using Firebase;
using System;
using Firebase.Extensions;

namespace DiceGame.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _id;
        [SerializeField] TMP_InputField _pw;
        [SerializeField] Button _tryLogin;
        [SerializeField] Button _register;

        private async void Start()
        {
            var dependencyState = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyState != DependencyStatus.Available)
            {
                throw new Exception();
            }

            _tryLogin.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(_id.text))
                    return;

                if (string.IsNullOrEmpty(_pw.text))
                    return;

                FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                auth.SignInWithEmailAndPasswordAsync(_id.text, _pw.text)
                    .ContinueWithOnMainThread(async task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("Canceled login");
                        }
                        else if (task.IsFaulted)
                        {
                            Debug.LogError("Faulted login");
                        }
                        else
                        {
                            Debug.Log("ID PW is correct");
                            bool result = await LoginInformation.RefreshInformationAsync(_id.text);

                            if (result == false)
                            {
                                UIManager.instance.Get<UINicknameSettingWindow>().Show();
                            }
                        }
                    });
            });

            _register.onClick.AddListener(() =>
            {
                UIManager.instance.Get<UIRegisterWindow>().Show();
            });
        }
    }
}