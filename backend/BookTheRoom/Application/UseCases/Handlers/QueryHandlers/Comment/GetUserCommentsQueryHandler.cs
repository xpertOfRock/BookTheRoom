using Application.UseCases.Queries.Comment;

namespace Application.UseCases.Handlers.QueryHandlers.Comment
{
    public class GetUserCommentsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserCommentsQuery, List<Core.Entities.Comment>>
    {
        public async Task<List<Core.Entities.Comment>> Handle(GetUserCommentsQuery query, CancellationToken cancellationToken)
        {
            return await unitOfWork.Comments.GetUserComments(query.UserId, query.Filter, cancellationToken);
        }
    }
}
