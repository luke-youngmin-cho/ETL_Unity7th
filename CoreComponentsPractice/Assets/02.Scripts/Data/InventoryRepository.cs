using System;
using System.Collections.Generic;

namespace DiceGame.Data
{
    public class InventoryRepository : IRepository<InventorySlotDataModel>
    {
        public InventoryRepository(InGameContext context)
        {
            _context = context;
        }

        private InGameContext _context;

        public event Action<int, InventorySlotDataModel> onItemUpdated;
        // event 의 +=, -= 연산에 대해 재정의가 필요하다면 다음처럼 정의할 수 있다. (보통은 event wrapping 에 사용한다)
        //{
        //    add
        //    {
        //        _context.onInventorySlotDataChanged += value;
        //    }
        //    remove
        //    {
        //        _context.onInventorySlotDataChanged -= value;
        //    }
        //}


        public void DeleteItem(InventorySlotDataModel item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InventorySlotDataModel> GetAllItems()
        {
            return _context.inventorySlotDataModels;
        }

        public InventorySlotDataModel GetItemByID(int id)
        {
            return _context.inventorySlotDataModels[id];
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
            _context.SaveInventorySlotDataModel(id, item, (dataChanged) => onItemUpdated?.Invoke(id, dataChanged));
        }
    }
}