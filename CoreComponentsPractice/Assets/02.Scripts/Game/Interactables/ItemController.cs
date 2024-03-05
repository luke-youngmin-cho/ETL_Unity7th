using UnityEngine;

namespace DiceGame.Game.Interactables
{
    public class ItemController : MonoBehaviour
    {
        [field: SerializeField] public int itemID { get; private set; }
        [field: SerializeField] public int itemNum { get; private set; }


        public void Interaction()
        {
            PickUp();
        }

        private void PickUp()
        {
            var inventoryRepository = GameManager.instance.unitOfWork.inventoryRepository;

            // todo -> 
            // 저장소를 전체 순회하면서 
            // 현재 ID 의 아이템을 차례대로 itemInfo.numMax 까지 채우는것을 시도하면서 모든 itemNum 을 가능한만큼 소모하고
            // 남은 갯수가 있으면 DB 를 갱신한 후에 itemNum 남은갯수로 갱신.

            if (itemNum == 0)
                Destroy(gameObject);
        }
    }
}