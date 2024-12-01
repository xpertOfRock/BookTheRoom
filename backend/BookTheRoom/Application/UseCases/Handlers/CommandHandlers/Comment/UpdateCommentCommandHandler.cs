using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using Braintree;
using Core.Interfaces;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.Comments.Update(request.Id, request.Description);

                if (!result.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return result;
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
        }
    }
}
