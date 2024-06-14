using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;


namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly IPhotoService _photoService;
        private readonly IMemoryCache _memoryCache;
        public HotelRepository(ApplicationDbContext context, IMemoryCache memoryCache,IPhotoService photoService) : base(context)
        {
            _memoryCache = memoryCache;
            _photoService = photoService;
        }
   
        public async Task<List<Hotel>> GetAllHotelsByAddress(string? country, string? city, string? streetOrDistrict, int? index)
        {
            var query = ApplicationDbContext.Hotels.Where(h => h.Address.Country == country);

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
                    return ApplicationDbContext.Hotels.Include(h => h.Address)
                                                      .Include(h => h.Rooms)
                                                      .AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                });          
        }
        public override async Task Update(Hotel hotel)
        {

            string key = $"hotel-{hotel.Id}";

            _memoryCache.Remove(key);
            
            ApplicationDbContext.Hotels.Update(hotel);
          
            _memoryCache.Set(key, hotel, TimeSpan.FromMinutes(2));
        }
        public override async Task Delete(Hotel hotel)
        {
            _memoryCache.Remove($"hotel-{hotel.Id}");
            await _photoService.DeletePhotoAsync(hotel.PreviewURL);

            foreach(var item in hotel.ImagesURL)
            {
                await _photoService.DeletePhotoAsync(item);
            }

            ApplicationDbContext.Hotels.Remove(hotel);
        }

        public async override Task<List<Hotel>> GetAll()
        {
            return await ApplicationDbContext.Hotels.Include(h => h.Address).ToListAsync();
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}
