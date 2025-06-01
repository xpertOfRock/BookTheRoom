namespace Infrastructure.Data.Repositories
{
    public class ApartmentRepository(
        ApplicationDbContext context,
        IDistributedCache distributedCache,
        IPhotoService photoService) : IApartmentRepository
    {
        public async Task<IResult> Add(Apartment apartment, CancellationToken token = default)
        {

            var existingApartment = await context.Apartments
                .AsNoTracking()
                .Include(a => a.Address)
                .FirstOrDefaultAsync(a => a.Address.Country == apartment.Address.Country &&
                                          a.Address.State == apartment.Address.State &&
                                          a.Address.City == apartment.Address.City &&
                                          a.Address.Street == apartment.Address.Street &&
                                          a.Address.PostalCode == apartment.Address.PostalCode, token);

            if (existingApartment is not null)
            {
                return new Fail("Entity with this address already exists.");
            }

            await context.Apartments.AddAsync(apartment, token);

            return new Success("New entity 'Apartment' created successfully.");
        }

        public async Task<IResult> Delete(int id, string userId, CancellationToken token = default)
        {
            var apartment = await context.Apartments.FirstOrDefaultAsync(a => a.Id == id, token);

            if (apartment == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
            }

            var key = $"apartment-{id}";

            await distributedCache.RemoveAsync(key, token);

            if (apartment.Images != null && apartment.Images.Count > 0)
            {
                foreach (var image in apartment.Images)
                {
                    await photoService.DeletePhotoAsync(image);
                }
            }

            var rawsAffected = await context.Apartments
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(token);

            if (rawsAffected == 0) throw new EntityNotFoundException<Apartment>();

            return new Success("Entity 'Apartment' was deleted successfully.");
        }
        public async Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request, CancellationToken token = default)
        {
            var query = context.Apartments
                .Include(a => a.Address)
                .Include(a => a.Comments)
                .Where(a => a.OwnerId == userId)
                .Where(a => string.IsNullOrWhiteSpace(request.Search) ||
                            a.Title.ToLower().Contains(request.Search.ToLower()) ||
                            a.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
                            a.Address.State.ToLower().Contains(request.Search.ToLower()) ||
                            a.Address.City.ToLower().Contains(request.Search.ToLower()))
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Countries))
            {
                var countries = request.Countries.Split(',');
                query = query.Where(a => countries.Contains(a.Address.Country));
            }

            if (request.MinPrice is not null && request.MinPrice >= 0)
            {
                query = query.Where(x => x.PriceForNight >= request.MinPrice);
            }

            if (request.MaxPrice is not null && request.MinPrice >= 1m)
            {
                query = query.Where(x => x.PriceForNight <= request.MaxPrice);
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

            query = query
                .Skip((request.Page - 1) * request.ItemsCount)
                .Take(request.ItemsCount);

            return await query.ToListAsync(token);
        }
        public async Task<List<Apartment>> GetAll(GetApartmentsRequest request, CancellationToken token = default)
        {
            var query = context.Apartments
                .Include(h => h.Address)               
                .Include(h => h.Comments)
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

            if (request.MinPrice is not null && request.MinPrice >= 0)
            {
                query = query.Where(x => x.PriceForNight >= request.MinPrice);
            }

            if (request.MaxPrice is not null && request.MinPrice >= 1m)
            {
                query = query.Where(x => x.PriceForNight <= request.MaxPrice);
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

            query = query.Skip((request.Page - 1) * request.ItemsCount).Take(request.ItemsCount);
            return await query.ToListAsync();
        }

        public async Task<Apartment> GetById(int id, CancellationToken token = default)
        {
            string key = $"apartment-{id}";
            if (key == $"apartment-2") await distributedCache.RemoveAsync(key, token);
            string? cachedApartment = await distributedCache.GetStringAsync(key, token);

            Apartment? apartment;

            if (string.IsNullOrEmpty(cachedApartment))
            {
                apartment = await context.Apartments
                    .Include(h => h.Address)
                    .Include(h => h.Comments)
                    .Include(h => h.Chats)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.Id == id, token);

                if (apartment is null)
                {
                    throw new EntityNotFoundException<Apartment>();
                }

                await distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(apartment),
                    token
                );

                return apartment;
            }

            apartment = JsonConvert.DeserializeObject<Apartment>(cachedApartment);

            if (apartment is null)
            {
                throw new EntityNotFoundException<Apartment>();
            }

            return apartment;
        }
        public async Task UpdateCache(Apartment apartment, CancellationToken token = default)
        {
            string key = $"apartment-{apartment.Id}";

            string? cachedApartment = await distributedCache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(cachedApartment)) return;

            await distributedCache.SetStringAsync
            (
                key,
                JsonConvert.SerializeObject(apartment),
                token
            );
        }
        public async Task<IResult> Update(int id, string userId, UpdateApartmentRequest request, CancellationToken token = default)
        {           
            var apartment = await GetById(id, token);

            if(apartment is null)
            {
                throw new EntityNotFoundException<Apartment>();
            }

            if(apartment.OwnerId != userId)
            {
                return new Fail("Mismatch between OwnerId property and passed value.");
            }

            string key = $"apartment-{id}";
           
            await distributedCache.RemoveAsync(key, token);

            if (request.Images is not null)
            {
                if (apartment.Images != null && apartment.Images.Any())
                {
                    foreach (var image in apartment.Images)
                    {
                        await photoService.DeletePhotoAsync(image);
                    }
                }

                await context.Apartments
                        .Where(a => a.Id == id)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(a => a.Images, request.Images), token);
            }

            await context.Apartments
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(e => e
                .SetProperty(a => a.Title, request.Title)
                .SetProperty(a => a.Description, request.Description)
                .SetProperty(a => a.PriceForNight, request.Price)
                .SetProperty(a => a.Address.Country, request.Address.Country)
                .SetProperty(a => a.Address.State, request.Address.State)
                .SetProperty(a => a.Address.City, request.Address.City)
                .SetProperty(a => a.Address.Street, request.Address.Street)
                .SetProperty(a => a.Address.PostalCode, request.Address.PostalCode), token);

            return new Success("Entity 'Apartment' was updated successfully.");
        }

        public async Task<IResult> UpdateUserDataInUserApartments(string userId, UpdateUserDataInUserApartmentsRequest request, CancellationToken token = default)
        {
            var affectedRows = await context.Apartments
                .Where(x => x.OwnerId == userId)
                .ExecuteUpdateAsync(e => e
                .SetProperty(x => x.PhoneNumber, request.PhoneNumber)
                .SetProperty(x => x.Email, request.Email)
                .SetProperty(x => x.OwnerName, request.OwnerName), token);

            var apartmentIds = await context.Apartments
                .AsNoTracking()
                .Where(x => x.OwnerId == userId)
                .Select(x => x.Id)              
                .ToListAsync(token);

            foreach(var apartmentId in apartmentIds)
            {
                string key = $"apartment-{apartmentId}";
                await distributedCache.RemoveAsync(key);
            }         

            string message = affectedRows == 0 ? "There are no apartments to update." : $"{affectedRows} apartment(-s) updated.";

            return new Success(message);
        }
    }
}
