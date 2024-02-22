using System;
using System.Collections;
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

        public static void Practice()
        {
            IEnumerator<Node> enumerator = nodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }
            enumerator.Reset();

            IEnumerable<NormalNode> normalNodes = new List<NormalNode>();
            IEnumerable<Node> baseNodes = new List<Node>();

            baseNodes = normalNodes; //공변성
            baseNodes = new List<NormalNode>();
            IEnumerable<Node> nodes2 = new List<NormalNode>();

            IDummy<A> dummy1 = new C<A>();
            IDummy<B> dummy2 = new C<B>();
            dummy1 = dummy2;
            stringAction = objectAction;

            string name = "Luke";
            object obj = name;
            obj = 1;
            object obj2 = obj;
            int a = (int)obj;
            Type type = typeof(string);

            stringAction += DoSomething2;
            stringAction += DoSomething2;
            stringAction += DoSomething2;
            stringAction += DoSomething2;
            stringAction += DoSomething2;
            stringAction.Invoke("Carl");

            //objectAction += DoSomething1;
            objectAction.Invoke(123123);
        }

        static Action<string> stringAction;
        static Action<object> objectAction;


        public static void DoSomething1(string str) { }
        public static void DoSomething2(object obj) { }


        public class A : object {}

        public class B : A {}

        public class C<T> : IDummy<T> {}

        public interface IDummy<out T> { }
    }
}
