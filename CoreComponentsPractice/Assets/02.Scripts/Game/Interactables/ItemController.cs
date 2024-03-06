using System.Linq;
using UnityEngine;
using DiceGame.Data;

namespace DiceGame.Game.Interactables
{
    public class ItemController : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public int itemID { get; private set; }
        [field: SerializeField] public int itemNum { get; private set; }


        public void SetUp(InventorySlotDataModel slotData)
        {
            itemID = slotData.itemID;
            itemNum = slotData.itemNum;
        }

        public void Interaction()
        {
            PickUp();
        }

        private void PickUp()
        {
            var inventoryRepository = GameManager.instance.unitOfWork.inventoryRepository;
            int slotID = 0;
            int maxNum = ItemInfoResources.instance[itemID].maxNum;
            int remains = itemNum;

            foreach (var slotData in inventoryRepository.GetAllItems().ToList())
            {
                if (slotData.isEmpty ||
                    (slotData.itemID == itemID && slotData.itemNum < maxNum))
                {
                    int vacancy = maxNum - slotData.itemNum;
                    remains = remains - vacancy;

                    // 다 채우고 남은 갯수가 양수면, 현재 슬롯은 최대치로 다 채우고도 남은것이므로 최대치로 설정
                    if (remains > 0)
                    {
                        inventoryRepository.UpdateItem(slotID, new InventorySlotDataModel(itemID, maxNum));
                    }
                    // 다 채우고 남은 갯수가 음수면, 다 못채웠으므로 최대치에서 다 못채운 양만큼 더해야함 (음수를 더함).
                    else if (remains < 0)
                    {
                        inventoryRepository.UpdateItem(slotID, new InventorySlotDataModel(itemID, maxNum + remains));
                        remains = 0;
                        break;
                    }
                    // 다 채우고 남은갯수가 0 이면 딱 현재슬롯까지 남은갯수로 다 채운상황.
                    else
                    {
                        inventoryRepository.UpdateItem(slotID, new InventorySlotDataModel(itemID, maxNum));
                        break;
                    }
                }

                slotID++;
            }

            if (remains == 0)
                Destroy(gameObject);
            else
                itemNum = remains;
        }
    }
}