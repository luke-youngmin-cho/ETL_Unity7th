using DiceGame.Game.Character;
using System;
using System.Collections;
using UnityEngine;

namespace DiceGame.Level
{
    public class DamageableObstacle : Obstacle, IHp
    {
        public float hp => _hp;

        public float hpMin => _hpMin;

        public float hpMax => _hpMax;

        private float _hp;
        private float _hpMin = 0.0f;
        [SerializeField] private float _hpMax = 100.0f;
        public event Action<float> onHpDepleted;


        protected override void Awake()
        {
            base.Awake();
            _hp = _hpMax;
        }

        public override IEnumerator C_Interaction(PlayerController interactor)
        {
            interactor.target = this;
            interactor.ChangeState(State.Attack);
            yield return new WaitUntil(() => interactor.state == State.Attack);
            yield return new WaitUntil(() => interactor.state == State.Move);
        }

        public void DepleteHp(float value)
        {
            if (_hp <= _hpMin || value <= 0)
                return;

            _hp -= value;
            _hp = Mathf.Clamp(_hp, _hpMin, _hpMax);
            onHpDepleted?.Invoke(_hp);

            if (_hp == 0)
            {
                node.obstacle = null;
                gameObject.SetActive(false);
            }
        }
    }
}