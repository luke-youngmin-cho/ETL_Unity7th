namespace DiceGame.Data
{
    /// <summary>
    /// 모든 Repository 참조를 가지고있는 단위
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 내부 Context 및 Repositories 준비 됐는지?
        /// </summary>
        public bool isReady { get; }

        /// <summary>
        /// InventoryRepository 참조 (Inventory UI 등에서 인벤토리의 정보가 필요할때 가져다 쓰기위함)
        /// </summary>
        IRepository<InventorySlotDataModel> inventoryRepository { get; }
    }
}