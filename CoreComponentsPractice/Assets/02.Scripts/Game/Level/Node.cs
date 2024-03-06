using DiceGame.Game.Character;
using DiceGame.Level.Items;
using System;
using UnityEngine;

namespace DiceGame.Level
{
    public abstract class Node : MonoBehaviour, INode, IComparable<Node>
    {
        public int nodeIndex => _nodeIndex;

        [field: SerializeField] public Item item { get; private set; }
        public Obstacle obstacle
        {
            get => _obstacle;
            set => _obstacle = value;
        }

        [SerializeField] private int _nodeIndex;
        [SerializeField] private Obstacle _obstacle;
        

        private void Awake()
        {
            BoardGameMap.Register(this);
            
            if (_obstacle)
                _obstacle.node = this;
        }

        public virtual void OnPlayerHere()
        {
            PlayerController.instance.direction = PlayerController.DIRECTION_POSITIVE;
            item?.Use(PlayerController.instance);
            item = null;
        }

        public virtual void OnDiceRolled(int diceValue)
        {
        }

        public int CompareTo(Node other)
        {
            if (_nodeIndex < other._nodeIndex)
                return -1;
            else if (_nodeIndex > other._nodeIndex)
                return 1;

            return 0;
        }
    }
}
