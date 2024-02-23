using DiceGame.Character;
using UnityEngine;

namespace DiceGame
{
    public class StateMachineBehaviourBase : StateMachineBehaviour
    {
        [SerializeField] State _state;
        private int _isDirtyAnimatorHashID = Animator.StringToHash("IsDirty");
        private PlayerController _controller;


        public virtual void Init(PlayerController controller)
        {
            _controller = controller;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            animator.SetBool(_isDirtyAnimatorHashID, false);
            _controller.state = _state;
        }
    }
}