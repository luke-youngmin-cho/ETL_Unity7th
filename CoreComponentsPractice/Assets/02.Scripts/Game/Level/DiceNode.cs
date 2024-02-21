using DiceGame.Game;

namespace DiceGame.Level
{
    public class DiceNode : Node
    {
        public override void OnPlayerHere()
        {
            base.OnPlayerHere();
            DicePlayManager.instance.diceNumber++;
        }
    }
}
