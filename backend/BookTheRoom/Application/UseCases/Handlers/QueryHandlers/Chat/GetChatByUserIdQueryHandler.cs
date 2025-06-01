using Application.UseCases.Queries.Chat;

namespace Application.UseCases.Handlers.QueryHandlers.Chat
{
    public class GetChatByUserIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatByUserIdQuery, Core.Entities.Chat?>
    {
        public async Task<Core.Entities.Chat?> Handle(GetChatByUserIdQuery query, CancellationToken cancellationToken)
        {
            return await unitOfWork.Chats.GetExistingChatByUserId(query.UserId, query.ApartmentId);
        }
    }
}
