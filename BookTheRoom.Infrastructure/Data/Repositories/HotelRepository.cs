using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.Entities;
using BookTheRoom.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IMemoryCache _memoryCache;
        public HotelRepository(ApplicationDbContext context, IMemoryCache memoryCache,IPhotoService photoService) : base(context)
        {
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
   
        public async Task<List<Hotel>> GetAllHotelsByAddress(string? country, string? city, string? streetOrDistrict, int? index)
        {
            var query = _context.Hotels.Where(h => h.Address.Country == country);

            if (city != null)
                query = query.Where(h => h.Address.City == city);
            if (streetOrDistrict != null)
                query = query.Where(h => h.Address.StreetOrDistrict == streetOrDistrict);
            if (index != null)
                query = query.Where(h => h.Address.Index == index);

            return await query.ToListAsync();            
        }
        public override async Task<Hotel> GetById(int id)
        {
            string key = $"hotel-{id}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _context.Hotels.Include(h => h.Address)
                                          .Include(h => h.Rooms)
                                          .AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                });          
        }
        
        public override void Update(Hotel hotel)
        {

            string key = $"hotel-{hotel.Id}";

            _memoryCache.Remove(key);

            _context.Hotels.Update(hotel);
          
            _memoryCache.Set(key, hotel, TimeSpan.FromMinutes(2));
        }
        public override async void Delete(Hotel hotel)
        {
            _memoryCache.Remove($"hotel-{hotel.Id}");
            await _photoService.DeletePhotoAsync(hotel.PreviewURL);

            foreach(var item in hotel.ImagesURL)
            {
                await _photoService.DeletePhotoAsync(item);
            }

            _context.Hotels.Remove(hotel);
        }

        public async override Task<List<Hotel>> GetAll()
        {
            return await _context.Hotels.Include(h => h.Address).ToListAsync();
        }

    }
}
