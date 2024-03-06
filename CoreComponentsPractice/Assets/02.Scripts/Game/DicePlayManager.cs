using DiceGame.UI;
using DiceGame.Game.Character;
using System;
using System.Collections;
using DiceGame.Level;
using DiceGame.Singleton;
using UnityEngine;
using NUnit.Framework;

namespace DiceGame.Game
{
    public class DicePlayManager : SingletonMonoBase<DicePlayManager>
    {
        public int diceNumber
        {
            get => _diceNumber;
            set
            {
                if (_diceNumber == value)
                    return;

                _diceNumber = value;
                onDiceNumberChanged?.Invoke(_diceNumber);
            }
        }

        private int _diceNumber = 3;
        private bool _isCorouting;
        public event Action<int> onDiceNumberChanged;
        public event Action onRollingDiceStarted;
        public event Action onRollingDiceFinished;


        public void RollADice()
        {
            if (_isCorouting)
                return;

            diceNumber--;
            onRollingDiceStarted?.Invoke();
            int diceValue =  UnityEngine.Random.Range(1, 7);
            _isCorouting = true;
            StartCoroutine(C_RollADice(diceValue));
        }

        IEnumerator C_RollADice(int diceValue)
        {
            yield return StartCoroutine(DiceRollingAnimationUI.instance.C_Animation(diceValue));
            BoardGameMap.nodes[PlayerController.instance.nodeIndex].OnDiceRolled(diceValue);
            yield return StartCoroutine(PlayerController.instance.C_Move(diceValue));
            BoardGameMap.nodes[PlayerController.instance.nodeIndex].OnPlayerHere();
            onRollingDiceFinished?.Invoke();
            _isCorouting = false;
        }

        public void EnumConcept()
        {
            MyCollection myCollection = new MyCollection(new ArrayList() { 1, "철수", 5.0f });

            IEnumerator e = myCollection.GetEnumerator();
            while (e.MoveNext())
            {
                Console.WriteLine(e.Current);
            }
            e.Reset();

            foreach (var item in myCollection)
            {
                Console.WriteLine(item);
            }

            foreach (var item in C_Something())
            {

            }
        }

        public class MyCollection : IEnumerable
        {
            public MyCollection(ArrayList data)
            {
                _data = data;
            }

            private ArrayList _data;

            public IEnumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            public struct Enumerator : IEnumerator
            {
                public Enumerator(MyCollection list)
                {
                    _current = null;
                    _items = list._data;
                    _index = 0;
                }

                public object Current => _current;
                private object _current;
                private readonly ArrayList _items;
                private int _index;

                public bool MoveNext()
                {
                    if (_index < _items.Count)
                    {
                        _current = _items[_index++];
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    _index = 0;
                    _current = default;
                }
            }
        }
        

        IEnumerable C_Something()
        {
            yield return 1;
            yield return "철수";
            yield return 5.0f;
        }
    }
}