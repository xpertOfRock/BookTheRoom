using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Identity;

namespace BookTheRoom.Infrastructure.Data.Repositories
{
    internal class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
