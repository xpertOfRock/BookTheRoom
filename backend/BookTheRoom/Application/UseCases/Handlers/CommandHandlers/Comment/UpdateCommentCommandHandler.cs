using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using Core.Enums;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Comments.Update(request.Id, request.Description);
            return Unit.Value;
        }
    }
}
