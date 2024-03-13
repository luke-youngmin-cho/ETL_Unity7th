using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DiceGame.Data;
using Firebase.Extensions;

namespace DiceGame.UI
{
    public class UINicknameSettingWindow : UIPopUpBase
    {
        private TMP_InputField _nickname;
        private Button _confirm;


        protected override void Awake()
        {
            base.Awake();

            _nickname = transform.Find("Panel/Panel - Nickname/InputField (TMP)").GetComponent<TMP_InputField>();
            _confirm = transform.Find("Panel/Button - Confirm").GetComponent<Button>();

            _confirm.onClick.AddListener(() =>
            {
                CollectionReference collectionReference = FirebaseFirestore.DefaultInstance.Collection("users");
                collectionReference.GetSnapshotAsync()
                                   .ContinueWithOnMainThread(task =>
                                   {
                                       // nickname 중복검사
                                       foreach (DocumentSnapshot documentSnapshot in task.Result.Documents)
                                       {
                                           if (documentSnapshot.GetValue<string>("nickname") == _nickname.text)
                                           {
                                               UIManager.instance.Get<UIWarningWindow>()
                                                                 .Show($"{_nickname.text} is already in use.");
                                               return;
                                           }
                                       }

                                       FirebaseFirestore.DefaultInstance.Collection("users").Document(LoginInformation.userKey)
                                            .SetAsync(new Dictionary<string, object>
                                            {
                                                {"nickname", _nickname.text },
                                            })
                                            .ContinueWith(task =>
                                            {
                                                LoginInformation.profile = new ProfileDataModel
                                                {
                                                    nickname = _nickname.text,
                                                };
                                            });
                                   });
            });

            _confirm.interactable = false;
            _nickname.onValueChanged.AddListener(value => _confirm.interactable = IsFormatValid());
        }

        private bool IsFormatValid()
        {
            return (_nickname.text.Length >= 2) && (_nickname.text.Length <= 10);
        }
    }
}