namespace DiceGame.Data
{
    /// <summary>
    /// 실제 사용할 저장소들의 참조를 구현
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            _context = new InGameContext();
            inventoryRepository = new InventoryRepository(_context); // context 에 대한 의존성주입을 생성자를 통해서 수행
        }

        public bool isReady => _context.hasInitialized;
        public IRepository<InventorySlotDataModel> inventoryRepository { get; private set; }


        private InGameContext _context;
    }
}