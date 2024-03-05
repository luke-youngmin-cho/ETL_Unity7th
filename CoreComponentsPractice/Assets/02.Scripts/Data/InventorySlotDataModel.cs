using Newtonsoft.Json;
using System;

namespace DiceGame.Data
{
    [Serializable]
    public class InventorySlotDataModel : IEquatable<InventorySlotDataModel>
    {
        [JsonConstructor]
        public InventorySlotDataModel() { }

        public InventorySlotDataModel(int itemID, int itemNum)
        {
            this.itemID = itemID;
            this.itemNum = itemNum;
        }

        public InventorySlotDataModel(InventorySlotDataModel copy)
        {
            this.itemID = copy.itemID;
            this.itemNum = copy.itemNum;
        }


        [JsonIgnore]
        public bool isEmpty => itemID == 0 || itemNum == 0;

        public int itemID;
        public int itemNum;


        public bool Equals(InventorySlotDataModel other)
        {
            return (this.itemID == other.itemID) && (this.itemNum == other.itemNum);
        }
    }
}