using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace DiceGame.Data.Mock
{
    public class MockInventoryRepository : IRepository<InventorySlotDataModel>
    {
        public MockInventoryRepository()
        {
            _path = Application.persistentDataPath + "/Inventory.json";

            if (File.Exists(_path))
            {
                _inventorySlotDataModels =
                    JsonConvert.DeserializeObject<List<InventorySlotDataModel>>(File.ReadAllText(_path));
            }
            else
            {
                _inventorySlotDataModels = new List<InventorySlotDataModel>(DEFAULT_CAPACITY);
                for (int i = 0; i < DEFAULT_CAPACITY; i++)
                {
                    _inventorySlotDataModels.Add(new InventorySlotDataModel(0, 0));
                }
                File.WriteAllText(_path, JsonConvert.SerializeObject(_inventorySlotDataModels));
            }
        }

        private readonly string _path;
        private const int DEFAULT_CAPACITY = 30;
        private List<InventorySlotDataModel> _inventorySlotDataModels;

        public event Action<int, InventorySlotDataModel> onItemUpdated;


        public IEnumerable<InventorySlotDataModel> GetAllItems()
        {
            return _inventorySlotDataModels;
        }

        public InventorySlotDataModel GetItemByID(int id)
        {
            return _inventorySlotDataModels[id];
        }

        public void InsertItem(InventorySlotDataModel item)
        {
            _inventorySlotDataModels.Add(item);
            Save();
        }

        public void DeleteItem(InventorySlotDataModel item)
        {
            _inventorySlotDataModels.Remove(item);
            Save();
        }

        public void UpdateItem(int id, InventorySlotDataModel item)
        {
            _inventorySlotDataModels[id] = item;
            Save();
            onItemUpdated?.Invoke(id, item);
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(_inventorySlotDataModels));
        }
    }
}