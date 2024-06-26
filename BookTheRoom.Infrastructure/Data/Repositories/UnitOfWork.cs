﻿using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.Interfaces;
using BookTheRoom.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IPhotoService _photoService;

        public UnitOfWork(ApplicationDbContext context, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _photoService = photoService;
            Hotels = new HotelRepository(_context, _memoryCache, _photoService);
            Rooms = new RoomRepository(_context, _memoryCache, _photoService);
            Orders = new OrderRepository(_context, _memoryCache);
            Addresses = new AddressRepository(_context);
        }

        public IHotelRepository Hotels { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IOrderRepository Orders { get; private set; }

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
