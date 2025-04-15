namespace Application.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface to manage repository access and transactions.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the apartment repository.
        /// </summary>
        IApartmentRepository Apartments { get; }

        /// <summary>
        /// Gets the hotel repository.
        /// </summary>
        IHotelRepository Hotels { get; }

        /// <summary>
        /// Gets the room repository.
        /// </summary>
        IRoomRepository Rooms { get; }

        /// <summary>
        /// Gets the order repository.
        /// </summary>
        IOrderRepository Orders { get; }

        /// <summary>
        /// Gets the comment repository.
        /// </summary>
        ICommentRepository Comments { get; }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Saves all changes made in the context.
        /// </summary>
        Task SaveChangesAsync();
    }
}
