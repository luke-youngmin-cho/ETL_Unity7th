using System;
using System.Linq;

namespace AISystems
{
    public class Parallel : Composite
    {
        public Parallel(BehaviourTree tree, int successCountRequired) : base(tree)
        {
            _successCountRequired = successCountRequired;
        }

        private int _successCountRequired; // Success policy
        private int _successCount;

        public override Result Invoke()
        {
            Result result = Result.Failure;

            for (int i = currentChildIndex; i < children.Count; i++)
            {
                result = children[i].Invoke();

                switch (result)
                {
                    case Result.Success:
                        {
                            _successCount++;
                        }
                        break;
                    case Result.Failure:
                        {
                        }
                        break;
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        break;
                }

                currentChildIndex++;
            }

            result = _successCount >= _successCountRequired ? Result.Success : Result.Failure;
            _successCount = 0;
            currentChildIndex = 0;
            return result;
        }
    }
}
