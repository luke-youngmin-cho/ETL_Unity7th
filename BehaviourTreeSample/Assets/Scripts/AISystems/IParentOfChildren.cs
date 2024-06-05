using System.Collections.Generic;

namespace AISystems
{
    public interface IParentOfChildren
    {
        public List<Node> children { get; }
    }
}
