using UnityEngine;

namespace DiceGame
{
    [CreateAssetMenu(fileName = "new ItemInfo", menuName = "ScriptableObjects/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public string description { get; private set; }
        [field: SerializeField] public int maxNum { get; private set; }
        [field: SerializeField] public Sprite icon { get; private set; }
    }
}