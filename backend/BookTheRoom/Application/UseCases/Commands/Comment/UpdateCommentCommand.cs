using Application.UseCases.Abstractions;

namespace Application.UseCases.Commands.Comment
{
    public class UpdateCommentCommand : ICommand<IResult>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public UpdateCommentCommand(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
