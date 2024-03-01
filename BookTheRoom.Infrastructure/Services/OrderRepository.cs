using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTheRoom.Infrastructure.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        //public Task Delete(Order order)
        //{
        //    _context.Orders.Remove(order);
        //    _context.SaveChanges();
        //    return Task.CompletedTask;
        //}

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        //public async Task<Order> GetOrderByIdAsync(int id)
        //{
        //    return await GetOrderByIdAsync
        //}

        //public Task Update(Order order)
        //{
        //    _context.Orders.Update(order);
        //    _context.SaveChanges();
        //    return Task.CompletedTask;
        //}
    }
}
