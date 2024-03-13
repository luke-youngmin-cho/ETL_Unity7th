using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;

namespace DiceGame.UI
{
    public class UIRegisterWindow : UIPopUpBase
    {
        private TMP_InputField _id;
        private TMP_InputField _pw;
        private Button _confirm;
        private Button _cancel;


        protected override void Awake()
        {
            base.Awake();

            _id = transform.Find("Panel/Panel - ID/InputField (TMP)").GetComponent<TMP_InputField>();
            _pw = transform.Find("Panel/Panel - PW/InputField (TMP)").GetComponent<TMP_InputField>();
            _confirm = transform.Find("Panel/Button - Confirm").GetComponent<Button>();
            _cancel = transform.Find("Panel/Button - Cancel").GetComponent<Button>();

            _confirm.onClick.AddListener(() =>
            {
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(_id.text, _pw.text)
                                            .ContinueWithOnMainThread(task =>
                                            {
                                                if (task.IsCanceled)
                                                {
                                                    Debug.LogError($"[UIRegisterWindow] : Canceled register {_id.text}");
                                                    return;
                                                }

                                                if (task.IsFaulted)
                                                {
                                                    if (task.Exception.Message.Contains("The email address is already in use by another account"))
                                                    {
                                                        UIManager.instance.Get<UIWarningWindow>()
                                                                          .Show($"Failed to register.. !\n{_id.text} is already in use.");
                                                    }
                                                    else
                                                    {
                                                        Debug.LogError($"[UIRegisterWindow] : Faulted register {_id.text}, {task.Exception}");
                                                    }

                                                    return;
                                                }

                                                AuthResult result = task.Result;
                                                UIManager.instance.Get<UIWarningWindow>()
                                                                  .Show($"Registered {_id.text}");
                                            });
            });
            _cancel.onClick.AddListener(Hide);

            _confirm.interactable = false;
            _id.onValueChanged.AddListener(value => _confirm.interactable = IsFormatValid());
            _pw.onValueChanged.AddListener(value => _confirm.interactable = IsFormatValid());
        }

        public override void Show()
        {
            base.Show();

            _id.text = string.Empty;
            _pw.text = string.Empty;
        }

        private bool IsFormatValid()
        {
            return (_id.text.IndexOf('@') > 0) && (_pw.text.Length >= 6);
        }
    }
}