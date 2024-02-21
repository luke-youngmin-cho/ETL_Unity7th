using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DiceGame.UI
{
    public class DiceRollingAnimationUI : MonoBehaviour
    {
        public static DiceRollingAnimationUI instance;

        [SerializeField] Sprite[] _diceSprites;
        [SerializeField] Sprite[] _diceRollingSprites;
        private Image _diceImage;
        [SerializeField] float _animationDuration;
        [SerializeField] float _animationSpeed;
        [SerializeField] float _animationDampingGain;
        [SerializeField] float _animationFinishDelay;


        private void Awake()
        {
            instance = this;

            _diceImage = transform.Find("Image - Dice").GetComponent<Image>();
        }

        public IEnumerator C_Animation(int diceValue)
        {
            float dampingRatio = 0.0f;
            float elapsedTime = 0.0f;

            while (elapsedTime < _animationDuration)
            {
                _diceImage.sprite = _diceRollingSprites[Random.Range(0, _diceRollingSprites.Length)];
                dampingRatio *= (1.0f + _animationDampingGain);
                elapsedTime += Time.deltaTime * dampingRatio / _animationSpeed;
                yield return new WaitForSeconds(Time.deltaTime * dampingRatio / _animationSpeed);
            }

            _diceImage.sprite = _diceSprites[diceValue - 1];
            yield return new WaitForSeconds(_animationFinishDelay);
            yield return null;
        }
    }
}
