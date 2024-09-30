using Application.Interfaces;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;
        

        public UnitOfWork
        (
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            IPhotoService photoService
        )
        {
            _userManager = userManager;
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
            
            Apartments = new ApartmentRepository(_context, _memoryCache, _photoService);
            Hotels = new HotelRepository(_context, _memoryCache, _photoService);
            Rooms = new RoomRepository(_context, _memoryCache, _photoService);
            Orders = new OrderRepository(_context, _memoryCache);
            Comments = new CommentRepository(_context, _userManager);
        }

        public IHotelRepository Hotels { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IApartmentRepository Apartments { get; private set; }
        public ICommentRepository Comments { get; private set; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
