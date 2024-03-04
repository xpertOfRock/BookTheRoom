using BookTheRoom.Application.Interfaces;


namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Hotels = new HotelRepository(_context);
            Rooms = new RoomRepository(_context);
            Orders = new OrderRepository(_context);
        }

        public IHotelRepository Hotels { get; private set; }

        public IRoomRepository Rooms { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
