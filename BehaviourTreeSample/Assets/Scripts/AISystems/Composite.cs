using System.Collections.Generic;

namespace AISystems
{
    public abstract class Composite : Node, IParentOfChildren
    {
        protected Composite(BehaviourTree tree) : base(tree)
        {
            children = new List<Node>();
        }

        public List<Node> children { get; set; }
        protected int currentChildIndex; // 자식이 Running 반환이후 빠져나올때, 이 Composite 는 다음 자식을 이어서 탐색해야하므로 탐색해야할 자식의 인덱스를 기억해야함.
    }
}
