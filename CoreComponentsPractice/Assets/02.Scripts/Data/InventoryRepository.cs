using System;
using System.Collections.Generic;

namespace DiceGame.Data
{
    public class InventoryRepository : IRepository<InventorySlotDataModel>
    {
        public event Action<int, InventorySlotDataModel> onItemUpdated;

        public void DeleteItem(InventorySlotDataModel item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InventorySlotDataModel> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public InventorySlotDataModel GetItemByID(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertItem(InventorySlotDataModel item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int id, InventorySlotDataModel item)
        {
            throw new NotImplementedException();
        }
    }
}