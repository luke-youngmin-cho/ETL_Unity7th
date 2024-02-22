using UnityEngine;
using TMPro;
using DiceGame.Game;

public class DiceNumberText : MonoBehaviour
{
    private TMP_Text _diceNumber;


    private void Start()
    {
        _diceNumber = GetComponent<TMP_Text>(); 
        _diceNumber.text = DicePlayManager.instance.diceNumber.ToString();
        DicePlayManager.instance.onDiceNumberChanged += (diceNumber) => _diceNumber.text = diceNumber.ToString();
    }
}
