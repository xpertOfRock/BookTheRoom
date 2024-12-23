using Newtonsoft.Json;

namespace Infrastructure.Data.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context;

        public ApartmentRepository(ApplicationDbContext context, IDistributedCache distributedCache, IPhotoService photoService)
        {
            _context = context;
            _distributedCache = distributedCache;
            _photoService = photoService;
        }

        private const int maxItemsOnPage = 15;

        public async Task<IResult> Add(Apartment apartment)
        {
            
            var existingApartment = _context.Apartments.AsNoTracking().FirstOrDefaultAsync(a => a.Address == apartment.Address);

            if (existingApartment is not null) 
            { 
                return new Fail("Entity with this address already exists.");
            }

            await _context.Apartments.AddAsync(apartment);

            return new Success("New entity 'Apartment' created successfuly.");
        }

        public async Task<IResult> Delete(int id)
        {
            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

            if (apartment == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
            }

            var key = $"apartment-{id}";
            _distributedCache.Remove(key);

            if (apartment.Images != null && apartment.Images.Count > 0)
            {
                foreach (var image in apartment.Images)
                {
                    await _photoService.DeletePhotoAsync(image);
                }
            }

            _context.Apartments.Remove(apartment);

            return new Success("Entity 'Apartment' was deleted successfuly.");
        }
        public async Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request)
        {
            var query = _context.Apartments
                .Include(h => h.Address)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Title.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Prices))
            {
                var prices = request.Prices.Split(',').Select(decimal.Parse).ToList();
                query = query.Where(h => prices[0] < h.PriceForNight && prices[1] < h.PriceForNight);
            }

            Expression<Func<Apartment, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "title" => apartment => apartment.Title,
                "price" => apartment => apartment.PriceForNight,
                _ => apartment => apartment.Id
            };

            query = request.SortOrder == "desc"
                ? query.OrderByDescending(selectorKey)
                : query.OrderBy(selectorKey);

            query = query.Skip((request.page - 1) * maxItemsOnPage).Take(maxItemsOnPage);

            return await query.ToListAsync();
        }
        public async Task<List<Apartment>> GetAll(GetApartmentsRequest request)
        {
            var query = _context.Apartments
                .Include(h => h.Address)                
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||                            
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if 
            (
                !string.IsNullOrWhiteSpace(request.Prices) &&
                request.Prices.Split(',').Select(decimal.Parse).ToList().Count == 2
            )
            {
                var prices = request.Prices
                    .Split(',')
                    .Select(decimal.Parse)
                    .ToList();
                query = query.Where(h => prices[0] < h.PriceForNight && prices[1] < h.PriceForNight);
            }

            Expression<Func<Apartment, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "title" => apartment => apartment.Title,
                "price" => apartment => apartment.PriceForNight,
                _ => apartment => apartment.Id
            };


            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);

            query = query.Skip((request.page - 1) * maxItemsOnPage).Take(maxItemsOnPage);

            return await query.ToListAsync();
        }

        public async Task<Apartment?> GetById(int? id, CancellationToken cancellationToken = default)
        {
            string key = $"apartment-{id}";
            string? cachedApartment = await _distributedCache.GetStringAsync(key, cancellationToken);

            Apartment? apartment;

            if (string.IsNullOrEmpty(cachedApartment))
            {
                apartment = await _context.Apartments
                    .Include(h => h.Address)
                    .Include(h => h.Comments)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id);

                if(apartment is null)
                {
                    return apartment;
                }

                await _distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(apartment),
                    cancellationToken
                    );

                return apartment;
            }

            apartment = JsonConvert.DeserializeObject<Apartment>(cachedApartment);
            return apartment;
        }
        public async Task<IResult> Update(int? id, UpdateApartmentRequest request)
        {           
            if(id is null)
            {
                throw new ArgumentNullException($"Cannot get entity '{nameof(Apartment)}' with '{id}' is null.");
            }

            var apartment = await GetById(id);

            if(apartment == null)
            {
                return new Fail("Impossible to update a non-existent entity.");
            }

            string key = $"apartment-{id}";
           
            _distributedCache.Remove(key);

            var comments = request.Comments == null ? apartment.Comments : new List<Comment>();

            if (request.Images is not null)
            {
                if (apartment.Images != null && apartment.Images.Any())
                {
                    foreach (var image in apartment.Images)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }

                await _context.Apartments
                        .Where(h => h.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }


            await _context.Apartments
                .Where(h => h.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(h => h.Title, request.Title)
                .SetProperty(h => h.Description, request.Description)
                .SetProperty(h => h.PriceForNight, request.Price));

            return new Success("Entity 'Apartment' was deleted successfuly.");
        }
    }
}
