namespace Application.UseCases.Commands.Chat
{
    public class CreateChatCommand : ICommand<Core.Entities.Chat>
    {

        public int? ApartmentId { get; } 
        public List<string> UsersId { get; }
        public CreateChatCommand(List<string> usersId, int? apartmentId = null)
        {
            UsersId = usersId;
            ApartmentId = apartmentId;
        }
    }
}
