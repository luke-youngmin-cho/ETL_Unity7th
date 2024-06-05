using System;

namespace AISystems
{
    public class Decorator : Node, IParentOfChild
    {
        public Decorator(BehaviourTree tree, Func<bool> condition) : base(tree)
        {
            _condition = condition;
        }

        public Node child { get; set; }
        private Func<bool> _condition;

        public override Result Invoke()
        {
            if (_condition.Invoke())
            {
                tree.stack.Push(child);
                return Result.Success;
            }

            return Result.Failure;
        }
    }
}
