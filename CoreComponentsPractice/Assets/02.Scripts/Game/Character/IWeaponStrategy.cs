using DiceGame.Game;
using DiceGame.Game.Effects;
using UnityEngine.Rendering;

namespace DiceGame.Character
{
    public interface IWeaponStrategy
    {
        WeaponType type { get; }
        void Attack(IHp target, float damage, out float amountDamaged);
    }
}