namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApartmentRepository Apartments { get; }
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IOrderRepository Orders { get; }
        ICommentRepository Comments { get; }
        IChatRepository Chats { get; }
        Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync(CancellationToken token = default);
    }
}
