using Core.Interfaces;

namespace Core.TasksResults
{
    public class Success : IResult
    {
        public bool IsSuccess => true;
        public string Message { get; }
        public Success(string message)
        {
            Message = message;
        }
    }
}
