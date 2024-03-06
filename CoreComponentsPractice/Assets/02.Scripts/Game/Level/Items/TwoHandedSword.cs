using DiceGame.Game.Character;
using DiceGame.Game;
using UnityEngine;

namespace DiceGame.Level.Items
{
    public class TwoHandedSword : ItemEquipment, IWeaponStrategy
    {
        public WeaponType type => WeaponType.TwoHandedSword;
        [SerializeField] float _damageGain = 1.5f;


        public void Attack(IHp target, float damage, out float amountDamaged)
        {
            amountDamaged = damage * _damageGain;
            target.DepleteHp(amountDamaged);
        }

        public override void Use(PlayerController controller)
        {
            controller.SetWeaponStrategy(this, transform);
        }
    }
}