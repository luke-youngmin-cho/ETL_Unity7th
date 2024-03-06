using DiceGame.Game.Character;

namespace DiceGame.Level
{
    public class InverseNode : Node
    {
        public override void OnPlayerHere()
        {
            PlayerController.instance.direction = PlayerController.DIRECTION_NEGATIVE;
        }
    }
}
