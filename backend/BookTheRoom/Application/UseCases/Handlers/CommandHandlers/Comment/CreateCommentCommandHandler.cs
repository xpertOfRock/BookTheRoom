using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Comments.Update(request.HotelId, request.Description);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
