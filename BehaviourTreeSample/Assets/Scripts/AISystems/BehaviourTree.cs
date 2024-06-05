using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystems
{
    public class BehaviourTree : MonoBehaviour
    {
        public Blackboard blackboard;
        public Stack<Node> stack = new Stack<Node>();
        public Root root;
        public bool isRunning;

        private void Update()
        {
            if (isRunning)
                return;

            isRunning = true;
            StartCoroutine(C_Tick());
        }

        IEnumerator C_Tick()
        {
            stack.Push(root);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();
                Result result = current.Invoke();

                if (result == Result.Running)
                {
                    stack.Push(current);
                    yield return null;
                }
            }

            isRunning = false;
        }

        #region Builder
        private Node _current;
        private Stack<Composite> _compositeBuiltStack;

        public BehaviourTree StartBuild()
        {
            blackboard = new Blackboard(gameObject);
            _compositeBuiltStack = new Stack<Composite>();
            _current = root = new Root(this);
            return this;
        }

        public BehaviourTree ExitCurrentComposite()
        {
            if (_compositeBuiltStack.Count > 0)
            {
                _compositeBuiltStack.Pop();
                _current = _compositeBuiltStack.Count > 0 ? _compositeBuiltStack.Peek() : null;
            }
            else
            {
                throw new Exception("[BehaviourTree] : Composite stack is empty.");
            }

            return this;
        }

        public BehaviourTree Selector()
        {
            Composite node = new Selector(this);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree RandomSelector()
        {
            Composite node = new RandomSelector(this);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree Sequence()
        {
            Composite node = new Sequence(this);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree Parallel(int successCountRequired)
        {
            Composite node = new Parallel(this, successCountRequired);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree Decorator(Func<bool> condition)
        {
            Node node = new Decorator(this, condition);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree Execution(Func<Result> execute)
        {
            Node node = new Execution(this, execute);
            Attach(_current, node);
            return this;
        }

        public BehaviourTree Seek(float radius, float height, float angle, LayerMask targetMask, float maxDistance)
        {
            Node node = new Seek(this, radius, height, angle, targetMask, maxDistance);
            Attach(_current, node);
            return this;
        }

        private void Attach(Node parent, Node child)
        {
            if (parent is IParentOfChild)
            {
                ((IParentOfChild)parent).child = child;
            }
            else if (parent is IParentOfChildren)
            {
                ((IParentOfChildren)parent).children.Add(child);
            }
            else
            {
                throw new System.Exception($"[BehaviourTree] : Can't attach child to {parent.GetType()}");
            }

            // Update Current 
            if (child is IParentOfChild)
            {
                _current = child;
            }
            else if (child is IParentOfChildren)
            {
                _current = child;
                // 자식 여러개인 컴포지트만 스택에 쌓는 이유는 자식 여러개 아니면 어차피 기억해놨다가 돌아가도 추가로 자식을 할당할수없기때문
                _compositeBuiltStack.Push((Composite)child); 
            }
            else
            {
                // 자식이 없는 노드는 _current 에 기억해봤자 자식붙일수 있는게 없으므로, 자식을 붙일수있는 가장 가까운 Composite 로 돌아감.
                _current = _compositeBuiltStack.Count > 0 ? _compositeBuiltStack.Peek() : null;
            }
        }


        #endregion
    }
}
