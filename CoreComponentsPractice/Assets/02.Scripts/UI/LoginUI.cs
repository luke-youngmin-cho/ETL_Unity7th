using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DiceGame.UI
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _id;
        [SerializeField] TMP_InputField _pw;
        [SerializeField] Button _login;

        private void Start()
        {
            _login.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(_id.text))
                    return;

                if (string.IsNullOrEmpty(_pw.text))
                    return;

                Login();
            });
        }

        private void Login()
        {
            SceneManager.LoadScene("DiceGame");
        }
    }
}