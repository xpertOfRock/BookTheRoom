using Application.UseCases.Commands.Comment;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{
    public class UpdateCommentCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateCommentCommand, IResult>
    {

        public async Task<IResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await unitOfWork.Comments.Update(request.Id, request.Description);

                if (!result.IsSuccess)
                {
                    await unitOfWork.RollbackAsync();
                    return result;
                }

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
        }
    }
}
