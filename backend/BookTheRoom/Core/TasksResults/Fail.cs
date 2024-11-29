using Core.Interfaces;

namespace Core.TasksResults
{
    public class Fail : IResult
    {
        public bool IsSuccess => false;

        public string Message { get; }
        public Fail(string message)
        {
            Message = message;
        }
    }
}
