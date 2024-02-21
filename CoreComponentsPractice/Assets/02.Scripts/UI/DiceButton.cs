using DiceGame.Game;
using UnityEngine;
using UnityEngine.UI;

namespace DiceGame.UI
{
    public class DiceButton : MonoBehaviour
    {
        private Button _button;


        private void Start()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                DicePlayManager.instance.RollADice();
            });

            DicePlayManager.instance.onDiceNumberChanged += (diceValue) =>
            {
                if (diceValue > 0)
                    ActiveButton();
                else
                    DeactiveButton();
            };
        }

        private void ActiveButton()
        {
            _button.interactable = true;
        }

        private void DeactiveButton()
        {
            _button.interactable = false;
        }
    }
}