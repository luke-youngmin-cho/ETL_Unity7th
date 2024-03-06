namespace DiceGame.Game.Character
{
    public interface IWeaponStrategy
    {
        WeaponType type { get; }
        void Attack(IHp target, float damage, out float amountDamaged);
    }
}