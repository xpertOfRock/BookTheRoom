using Newtonsoft.Json;

namespace Infrastructure.Data.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;
        
        public HotelRepository(ApplicationDbContext context, IDistributedCache distributedCache, IPhotoService photoService)
        {
            _context = context;
            _distributedCache = distributedCache;
            _photoService = photoService;
        }

        public async Task<IResult> Add(Hotel hotel)
        {
            Hotel? existingHotel = await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(h => h.Name == hotel.Name);

            if(existingHotel is not null)
            {
                return new Fail("Entity with this address and name already exists.");
            }

            await _context.Hotels.AddAsync(hotel);

            return new Success("Entity 'Hotel' was created successfuly.");
        }
        
        public async Task<IResult> Delete(int id)
        {            
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);

            if(hotel == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
            }

            _distributedCache.Remove($"hotel-{id}");

            if (hotel.Images != null && hotel.Images.Count > 0)
            {
                foreach (var image in hotel.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }
            
            _context.Hotels.Remove(hotel);
            return new Success("Entity 'Hotel' was deleted successfuly.");
        }
        
        public async Task<List<Hotel>> GetAll(GetHotelsRequest request)
        {
            var query = _context.Hotels
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Name.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Ratings))
            {
                var ratings = request.Ratings.Split(',').Select(int.Parse).ToList();
                query = query.Where(h => ratings.Contains(h.Rating));
            }

            if (!string.IsNullOrWhiteSpace(request.Services))
            {
                var services = request.Services.Split(',').Select(service => Enum.Parse<HotelService>(service, true));
                query = query.Where(h => h.Services.Any(x => services.Contains(x.ServiceName)));
            }

            Expression<Func<Hotel, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "name" => hotel => hotel.Name,
                "rating" => hotel => hotel.Rating,
                _ => hotel => hotel.Id
            };

            query = request.SortOrder == "desc"
                ? query.OrderByDescending(selectorKey)
                : query.OrderBy(selectorKey);

            query = query.Skip((request.Page - 1) * request.ItemsCount).Take(request.ItemsCount);

            return await query.ToListAsync();
        }


        public async Task<Hotel?> GetById(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null) throw new ArgumentNullException("Cannot get entity 'Hotel' when 'id' is null.");

            string key = $"hotel-{id}";
            string? cachedHotel = await _distributedCache.GetStringAsync(key, cancellationToken);

            Hotel? hotel;

            if (string.IsNullOrEmpty(cachedHotel))
            {
                hotel = await _context.Hotels
                    .Include(h => h.Address)
                    .Include(h => h.Rooms)
                    .Include(h => h.Comments)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);

                if (hotel is null)
                {
                    return hotel;
                }

                await _distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(hotel),
                    cancellationToken
                    );

                return hotel;
            }

            hotel = JsonConvert.DeserializeObject<Hotel>(cachedHotel);
            return hotel;
        }

        public async Task<IResult> Update(int id, UpdateHotelRequest request)
        {
            var hotel = await GetById(id);

            if (hotel == null)
            {
                return new Fail("Impossible to update a non-existent entity.");
            }

            string key = $"hotel-{id}";

            _distributedCache.Remove(key);

            if (request.Images is not null)
            {
                if (hotel.Images != null && hotel.Images.Any())
                {
                    foreach (var image in hotel.Images)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                await _context.Hotels
                        .Where(h => h.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }

            await _context.Hotels
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(h => h.Name, request.Name)
                .SetProperty(h => h.Description, request.Description)
                .SetProperty(h => h.Rating, request.Rating)
                .SetProperty(h => h.Address.Country, request.Address.Country)
                .SetProperty(h => h.Address.State, request.Address.State)
                .SetProperty(h => h.Address.City, request.Address.City)
                .SetProperty(h => h.Address.Street, request.Address.Street)
                .SetProperty(h => h.Address.PostalCode, request.Address.PostalCode)
                .SetProperty(h => h.HasPool, request.HasPool));

            return new Success("Entity 'Hotel' was updated successfuly.");
        }
    }
}
