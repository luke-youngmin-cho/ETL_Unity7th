using Extensions;
using System;
using System.Linq;

namespace AISystems
{
    public class RandomSelector : Composite
    {
        public RandomSelector(BehaviourTree tree) : base(tree)
        {
            _random = new Random();
        }

        private Random _random;

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
                            _random.Shuffle(children);
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

            _random.Shuffle(children);
            currentChildIndex = 0;
            return result;
        }
    }
}
