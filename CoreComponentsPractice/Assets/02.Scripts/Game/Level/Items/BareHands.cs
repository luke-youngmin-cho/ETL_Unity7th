using DiceGame.Game.Character;
using DiceGame.Game;
using UnityEngine;

namespace DiceGame.Level.Items
{
    public class BareHands : ItemEquipment, IWeaponStrategy
    {
        public WeaponType type => WeaponType.None;
        [SerializeField] float _damageGain = 1.0f;


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