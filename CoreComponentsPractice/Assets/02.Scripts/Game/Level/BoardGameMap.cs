using System.Collections.Generic;

namespace DiceGame.Level
{
    public static class BoardGameMap
    {
        public static List<Node> nodes = new List<Node>();


        public static void Register(Node node)
        {
            nodes.Add(node);
            nodes.Sort();
        }
    }
}
