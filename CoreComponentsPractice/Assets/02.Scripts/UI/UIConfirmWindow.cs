using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace DiceGame.UI
{
    public class UIConfirmWindow : UIPopUpBase
    {
        private TMP_Text _message;
        private Button _confirm;
        private Button _cancel;


        protected override void Awake()
        {
            base.Awake();

            _message = transform.Find("Panel/Text (TMP) - Message").GetComponent<TMP_Text>();
            _confirm = transform.Find("Panel/Button - Confirm").GetComponent<Button>();
            _cancel  = transform.Find("Panel/Button - Cancel").GetComponent<Button>();
        }

        public void Show(string message, UnityAction onConfirm, UnityAction onCancel = null)
        {
            _message.text = message;

            _confirm.onClick.RemoveAllListeners();
            _confirm.onClick.AddListener(Hide);

            if (onConfirm != null)
                _confirm.onClick.AddListener(onConfirm);

            _cancel.onClick.RemoveAllListeners();
            _cancel.onClick.AddListener(Hide);

            if (onCancel != null)
                _cancel.onClick.AddListener(onCancel);

            base.Show();
        }
    }
}