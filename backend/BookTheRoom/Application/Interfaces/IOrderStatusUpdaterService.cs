namespace Application.Interfaces
{
    /// <summary>
    /// Service for updating the status of orders.
    /// </summary>
    public interface IOrderStatusUpdaterService
    {
        /// <summary>
        /// Updates the statuses of orders based on internal logic. Used in OrderStatusUpdaterBackgroundService.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateOrderStatus();
    }
}
