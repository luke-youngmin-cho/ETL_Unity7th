using DiceGame.Game.Character;
using UnityEngine;

namespace DiceGame.Level.Items
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void Use(PlayerController controller);
    }
}