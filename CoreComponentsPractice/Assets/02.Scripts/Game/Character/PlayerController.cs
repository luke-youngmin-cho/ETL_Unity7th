using DiceGame.Game;
using DiceGame.Game.Effects;
using DiceGame.Level;
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
        public const int DIRECTION_NEGATIVE = -1;

        // 1 : positive, -1 : negative
        public int direction { get; set; }
        public int nodeIndex { get; set; }
        public float hp => _hp;
        public float hpMin => _hpMin;
        public float hpMax => _hpMax;
        public State state
        {
            get => _state;
            set => _state = value;
        }

        public IHp target { get; set; }

        private float _hp;
        private float _hpMin = 0.0f;
        private float _hpMax = 100.0f;
        [SerializeField] float _moveSpeed = 1.0f;
        private float _attackPower = 10.0f;
        private DamagePopUpFactory _damagePopUpFactory;
        private Animator _animator;
        private State _state;
        private int _stateAnimatorHashID = Animator.StringToHash("State");
        private int _isDirtyAnimatorHashID = Animator.StringToHash("IsDirty");
        private int _speedAnimatorHashID = Animator.StringToHash("Speed");
        private int _weaponAnimatorHashID = Animator.StringToHash("Weapon");
        private IWeaponStrategy _weaponStrategy;
        [SerializeField] Transform _rightHand;
        // event 한정자 : 외부 클래스에서는 이 대리자를 쓸 때 +=, -= 의 피연산자로만 사용가능
        public event Action<float> onHpDepleted;


        private void Awake()
        {
            instance = this;
            _hp = _hpMax;
            direction = DIRECTION_POSITIVE;
            _damagePopUpFactory = new DamagePopUpFactory();
            _animator = GetComponent<Animator>();
            var stateMachineBehaviours = _animator.GetBehaviours<StateMachineBehaviourBase>();
            for (int i = 0; i < stateMachineBehaviours.Length; i++)
            {
                stateMachineBehaviours[i].Init(this);
            }
        }

        private void Start()
        {
            if (_rightHand.childCount > 0)
            {
                _weaponStrategy = _rightHand.GetChild(0).GetComponent<IWeaponStrategy>();
                Transform weaponTransform = _rightHand.GetChild(0);
                weaponTransform.SetParent(_rightHand);
                weaponTransform.localPosition = Vector3.zero;
                weaponTransform.localRotation = Quaternion.identity;
                _animator.SetInteger(_weaponAnimatorHashID, (int)_weaponStrategy.type);
            }
        }

        public void DepleteHp(float value)
        {
            if (_hp <= _hpMin || value <= 0)
                return;

            _hp -= value;
            onHpDepleted?.Invoke(_hp);
        }

        public IEnumerator C_Move(int diceValue)
        {
            for (int i = 0; i < diceValue; i++)
            {
                int nextIndex = nodeIndex + direction;
                if (nextIndex < 0 || nextIndex >= BoardGameMap.nodes.Count)
                    break;

                // 장애물 확인
                if (BoardGameMap.nodes[nextIndex].obstacle)
                {
                    yield return StartCoroutine(BoardGameMap.nodes[nextIndex].obstacle.C_Interaction(this)); // 장애물과 상호작용 루틴 시작
                }
                else
                {
                    _animator.SetFloat(_speedAnimatorHashID, 1.0f);
                    float t = 0.0f;
                    while (t < 1.0f)
                    {
                        transform.position = Vector3.Lerp(BoardGameMap.nodes[nodeIndex].transform.position,
                                                          BoardGameMap.nodes[nextIndex].transform.position,
                                                          t);
                        t += _moveSpeed * Time.deltaTime;
                        yield return null;
                    }
                    nodeIndex = nextIndex;
                    _animator.SetFloat(_speedAnimatorHashID, 0.0f);
                }
            }
        }

        public void ChangeState(State newState)
        {
            _animator.SetInteger(_stateAnimatorHashID, (int)newState);
            _animator.SetBool(_isDirtyAnimatorHashID, true);
        }

        public void SetWeaponStrategy(IWeaponStrategy weaponStrategy, Transform weaponTransform)
        {
            if (_rightHand.childCount > 0)
                Destroy(_rightHand.GetChild(0).gameObject);

            weaponTransform.SetParent(_rightHand);
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.identity;
            _animator.SetInteger(_weaponAnimatorHashID, (int)weaponStrategy.type);
            _weaponStrategy = weaponStrategy;
        }

        private void Hit(AnimationEvent e)
        {
            if (target != null)
            {
                _weaponStrategy.Attack(target, _attackPower, out float amountDamaged);
                _damagePopUpFactory.Create(transform.position + Vector3.forward * 1.0f + Vector3.up * 1.2f,
                                           Quaternion.identity,
                                           amountDamaged,
                                           DamageType.Normal);
            }
        }

        [SerializeField] AudioSource _footStepAudioSource;
        private void FootL()
        {
            if (_footStepAudioSource.isPlaying)
                return;

            _footStepAudioSource.Play();
        }
        private void FootR()
        {
            if (_footStepAudioSource.isPlaying)
                return;

            _footStepAudioSource.Play();
        }
    }
}