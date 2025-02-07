using Newtonsoft.Json;

namespace Infrastructure.Data.Repositories
{
    public class RoomRepository
        (ApplicationDbContext context,
        IDistributedCache distributedCache,
        IPhotoService photoService) : IRoomRepository
    {
        public async Task<IResult> Add(Room room)
        {
            Room? existingRoom = context.Rooms.AsNoTracking().FirstOrDefault(r => r.HotelId == room.HotelId && r.Number == room.Number);

            if (existingRoom is not null)
            {
                return new Fail("Entity with this hotelId and number already exists.");
            }            
            await context.Rooms.AddAsync(room);

            return new Success("Entity 'Room' was created successfully.");
        }

        public async Task<IResult> Delete(int hotelId, int number)
        {
            var room = await GetById(hotelId, number);

            if(room == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
            }

            string key = $"hotel-{hotelId}-room-{number}";

            distributedCache.Remove(key);

            if (room.Images != null && room.Images.Count > 0)
            {
                foreach (var image in room.Images)
                {
                    await photoService.DeletePhotoAsync(image);
                }
            }

            context.Rooms.Remove(room);

            return new Success("Entity 'Room' was deleted successfully.");
        }
        public async Task<List<Room>> GetAllRooms()
        {
            return await context.Rooms
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Room>> GetAll(int hotelId, GetRoomsRequest request)
        {
            var query = context.Rooms
                .Where(r => r.HotelId == hotelId && 
                            (string.IsNullOrWhiteSpace(request.Search) ||
                            r.Name.ToLower().Contains(request.Search.ToLower()) ||
                            r.Description.ToLower().Contains(request.Search.ToLower())) 
                            )
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Categories))
            {
                var categories = request.Categories.Split(',');
                query = query.Where(r => categories.Contains(((int)r.Category).ToString()));
            }

            if (request.MinPrice is not null && request.MinPrice >= 0)
            {
                query = query.Where(r => r.Price >= request.MinPrice);
            }

            if (request.MaxPrice is not null && request.MinPrice >= 1m)
            {
                query = query.Where(r => r.Price <= request.MaxPrice);
            }

            Expression<Func<Room, object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "price" => room => room.Price,
                "category" => room => (int)room.Category,
                "number" => room => room.Number,
                _ => room => room.Name
            };

            query = request.SortOrder == "desc"
                 ? query = query.OrderByDescending(selectorKey)
                 : query = query.OrderBy(selectorKey);

            return await query.ToListAsync();
        }

        public async Task<Room?> GetById(int? hotelId, int? number, CancellationToken cancellationToken = default)
        {
            if (hotelId == null || number == null) throw new ArgumentNullException("Cannot get entity 'Hotel' when 'id' is null.");

            string key = $"hotel-{hotelId}-room-{number}";

            string? cachedHotel = await distributedCache.GetStringAsync(key, cancellationToken);

            Room? room;

            if (string.IsNullOrEmpty(cachedHotel))
            {
                room = await context.Rooms
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        r => r.HotelId == hotelId &&
                        r.Number == number);

                if (room is null)
                {
                    return room;
                }

                await distributedCache.SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(room),
                    cancellationToken
                );

                return room;
            }

            room = JsonConvert.DeserializeObject<Room>(cachedHotel);
            return room;
        }

        public async Task<IResult> Update(int hotelId, int number, UpdateRoomRequest request)
        {
            var room = await GetById(hotelId, number);

            if (room == null)
            {
                return new Fail("Impossible to update a non-existent entity.");
            }

            string key = $"hotel-{hotelId}-room-{number}";

            distributedCache.Remove(key);

            if (request.Images is not null && request.Images.Count > 0)
            {
                if (room.Images != null && room.Images.Any())
                {
                    foreach (var image in room.Images)
                    {
                        await photoService.DeletePhotoAsync(image);
                    }
                }
                await context.Rooms
                        .Where(h => h.HotelId == hotelId && h.Number == number)
                        .ExecuteUpdateAsync(e => e
                        .SetProperty(h => h.Images, request.Images));
            }

            await context.Rooms
                .Where(r => r.HotelId == hotelId && r.Number == number)
                .ExecuteUpdateAsync(e => e
                .SetProperty(r => r.Name, request.Name)
                .SetProperty(r => r.Description, request.Description)
                .SetProperty(r => r.Price, request.Price)
                .SetProperty(r => r.Category, request.Category));

            return new Success("Entity 'Room' was updated successfully.");
        }
    }
}
