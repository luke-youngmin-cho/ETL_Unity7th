using System;
using System.Linq;

namespace AISystems
{
    public class Selector : Composite
    {
        public Selector(BehaviourTree tree) : base(tree)
        {
        }

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
                            currentChildIndex = 0;
                            return result;
                        }
                    case Result.Failure:
                        {
                            currentChildIndex++;
                        }
                        break;
                    case Result.Running:
                        {
                            return result;
                        }
                    default:
                        break;
                }
            }

            currentChildIndex = 0;
            return result;
        }
    }
}
