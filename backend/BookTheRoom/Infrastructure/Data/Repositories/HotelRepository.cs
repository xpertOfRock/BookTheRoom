namespace Infrastructure.Data.Repositories
{
    public class HotelRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache,
        IPhotoService photoService) : IHotelRepository
    {
        public async Task<IResult> Add(Hotel hotel, CancellationToken token = default)
        {
            var existingHotel = await context.Hotels
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Name == hotel.Name, token);

            if(existingHotel is not null)
            {
                return new Fail("Entity with this address and name already exists.");
            }

            await context.Hotels.AddAsync(hotel, token);
            
            return new Success("Entity 'Hotel' was created successfully.");
        }
        
        public async Task<IResult> Delete(int id, CancellationToken token = default)
        {            
            var hotel = await context.Hotels.FirstOrDefaultAsync(h => h.Id == id, token);

            if(hotel == null) throw new EntityNotFoundException<Hotel>();

            await distributedCache.RemoveAsync($"hotel-{id}", token);

            if (hotel.Images != null && hotel.Images.Count > 0)
            {
                foreach (var image in hotel.Images)
                {
                    await photoService.DeletePhotoAsync(image);
                }
            }

            var RowsAffected = await context.Hotels
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(token);

            if (RowsAffected == 0) throw new EntityNotFoundException<Hotel>();

            return new Success("Entity 'Hotel' was deleted successfully.");
        }
        
        public async Task<List<Hotel>> GetAll(GetHotelsRequest request, CancellationToken token = default)
        {
            var query = context.Hotels
                .Include(h => h.Address)
                .Include(h => h.Comments)
                .Where(h => string.IsNullOrWhiteSpace(request.Search) ||
                            h.Name.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            h.Address.City.ToLower().Contains(request.Search.ToLower())
                            )

                .AsNoTracking();

            if(query.Select(h => h.Comments).Any() && query.Select(h => h.Comments) is not null && request.UserScore is not null)
            {
                query = query.Where(h => 
                    h.Comments!
                        .Where(c => c.UserScore != null && c.UserScore > 0)
                        .Average(c => c.UserScore) >= request.UserScore);                       
            }                

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');

                query = query.Where(h => countries.Contains(h.Address.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.Ratings))
            {
                var ratings = request.Ratings
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                query = query.Where(h => ratings.Contains(h.Rating));
            }

            if (!string.IsNullOrWhiteSpace(request.Services))
            {
                var services = request.Services
                    .Split(',')
                    .Select(service => Enum.Parse<HotelService>(service, true));

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

            query = query
                .Skip((request.Page - 1) * request.ItemsCount)
                .Take(request.ItemsCount);

            return await query.ToListAsync(token);
        }

        public async Task<Hotel?> GetById(int id, CancellationToken token = default)
        {
            string key = $"hotel-{id}";
            string? cachedHotel = await distributedCache.GetStringAsync(key, token);

            Hotel? hotel;
            
            if (string.IsNullOrEmpty(cachedHotel))
            {
                hotel = await context.Hotels
                    .Include(h => h.Address)
                    .Include(h => h.Rooms)
                    .Include(h => h.Comments)                    
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id,
                    token);

                if (hotel is null) throw new EntityNotFoundException<Hotel>();

                await distributedCache.SetStringAsync
                (
                    key,
                    JsonConvert.SerializeObject(hotel),
                    token
                );

                return hotel;
            }

            hotel = JsonConvert.DeserializeObject<Hotel>(cachedHotel);

            if (hotel is null) throw new EntityNotFoundException<Hotel>();

            return hotel;
        }
        public async Task UpdateCache(Hotel hotel, CancellationToken token = default)
        {
            string key  = $"hotel-{hotel.Id}";

            string? cachedHotel = await distributedCache.GetStringAsync(key, token);


            if (string.IsNullOrEmpty(cachedHotel)) return;

            await distributedCache.SetStringAsync
            (
                key,
                JsonConvert.SerializeObject(hotel),
                token
            );
        }
        public async Task<IResult> Update(int id, UpdateHotelRequest request, CancellationToken token = default)
        {
            var hotel = await GetById(id, token);

            if (hotel is null) throw new EntityNotFoundException<Hotel>();

            string key = $"hotel-{id}";

            await distributedCache.RemoveAsync(key, token);

            int affectedRows;

            if (request.Images is not null && request.Images.Any() && request.Images.Count > 0)
            {
                if (hotel.Images != null && hotel.Images.Any())
                {
                    foreach (var image in hotel.Images)
                    {
                        await photoService.DeletePhotoAsync(image);
                    }
                }
                affectedRows = await context.Hotels
                        .Where(h => h.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images),
                        token);
                if(affectedRows == 0) return new Fail("Entity 'Apartment' was not found.");
            }

            affectedRows = await context.Hotels
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
                .SetProperty(h => h.HasPool, request.HasPool),
                token);

            return affectedRows == 0 ? new Fail("Entity 'Hotel' was not found.") : new Success("Entity 'Hotel' was updated successfully.");
        }
    }
}
