using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        private readonly IPhotoService _photoService;
        

        public UnitOfWork
        (
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IDistributedCache memoryCache,
            IPhotoService photoService
        )
        {
            _userManager = userManager;
            _context = context;
            _distributedCache = memoryCache;
            _photoService = photoService;
            
            Apartments = new ApartmentRepository(_context, _distributedCache, _photoService);
            Hotels = new HotelRepository(_context, _distributedCache, _photoService);
            Rooms = new RoomRepository(_context, _distributedCache, _photoService);
            Orders = new OrderRepository(_context, _distributedCache);
            Comments = new CommentRepository(_context, _userManager);
            Chats = new ChatRepository(_context);
        }

        public IHotelRepository Hotels { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IApartmentRepository Apartments { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IChatRepository Chats { get; private set; }

        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CurrentTransaction!.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.CurrentTransaction!.RollbackAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
