using DiceGame.Game.Character;

namespace DiceGame.Level.Items
{
    public class PoisonPotion : Item
    {
        private float _damage = 10.0f;


        public override void Use(PlayerController controller)
        {
            controller.DepleteHp(_damage);
            Destroy(gameObject);
        }
    }
}