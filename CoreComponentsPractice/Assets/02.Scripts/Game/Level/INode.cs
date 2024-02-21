namespace DiceGame.Level
{
    public interface INode
    {
        void OnPlayerHere();
        void OnDiceRolled(int diceValue);
    }
}