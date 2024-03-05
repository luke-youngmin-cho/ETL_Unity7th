namespace DiceGame.Data
{
    /// <summary>
    /// 실제 사용할 저장소들의 참조를 구현
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            inventoryRepository = new InventoryRepository();
        }

        public IRepository<InventorySlotDataModel> inventoryRepository { get; private set; }
    }
}