namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApartmentRepository Apartments { get; }
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IOrderRepository Orders { get; }
        ICommentRepository Comments { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }
}
