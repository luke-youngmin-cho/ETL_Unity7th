using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.Character
{
    public class PlayerController : MonoBehaviour, IHp
    {
        public static PlayerController instance;

        public const int DIRECTION_POSITIVE = 1;
        public const int DIRECTION_NEGATIVE = 1;

        // 1 : positive, -1 : negative
        public int direction { get; set; }
        public int nodeIndex { get; set; }
        public float hp => _hp;
        public float hpMin => _hpMin;
        public float hpMax => _hpMax;

        private float _hp;
        private float _hpMin = 0.0f;
        private float _hpMax = 100.0f;
        // event 한정자 : 외부 클래스에서는 이 대리자를 쓸 때 +=, -= 의 피연산자로만 사용가능
        public event Action<float> onHpDepleted;


        private void Awake()
        {
            instance = this;
            _hp = _hpMax;
            direction = DIRECTION_POSITIVE;
        }

        public void DepleteHp(float amount)
        {
            if (_hp <= _hpMin || amount <= 0)
                return;

            _hp -= amount;
            onHpDepleted?.Invoke(_hp);
        }

        public IEnumerator C_Move(int diceValue)
        {
            yield return null;
        }
    }
}