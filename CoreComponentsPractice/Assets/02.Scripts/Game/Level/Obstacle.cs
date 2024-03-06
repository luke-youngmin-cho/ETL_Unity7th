using DiceGame.Game.Character;
using System;
using System.Collections;
using UnityEngine;

namespace DiceGame.Level
{
    public abstract class Obstacle : MonoBehaviour
    {
        public Node node { get; set; }

        protected virtual void Awake() { }
        public abstract IEnumerator C_Interaction(PlayerController interactor);
    }
}