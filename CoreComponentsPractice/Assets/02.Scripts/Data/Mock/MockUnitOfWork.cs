namespace DiceGame.Data.Mock
{
    /// <summary>
    /// 모조의 저장소들을 참조하기위한 (단위 테스트용) 단위.
    /// </summary>
    public class MockUnitOfWork : IUnitOfWork
    {
        public MockUnitOfWork()
        {
            inventoryRepository = new MockInventoryRepository();
        }


        public bool isReady => true;

        public IRepository<InventorySlotDataModel> inventoryRepository { get; private set; }
    }
}