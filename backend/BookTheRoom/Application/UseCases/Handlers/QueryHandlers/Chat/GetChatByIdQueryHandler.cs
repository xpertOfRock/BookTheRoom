using Application.UseCases.Queries.Chat;

namespace Application.UseCases.Handlers.QueryHandlers.Chat
{
    public class GetChatByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetChatByIdQuery, Core.Entities.Chat>
    {
        public async Task<Core.Entities.Chat> Handle(GetChatByIdQuery query, CancellationToken cancellationToken)
        {
            return await unitOfWork.Chats.GetChatById(query.Id, cancellationToken);
        }
    }
}
