using Core.Enums;
using Core.Interfaces;

namespace Core.TasksResults
{
    public class Fail : IResult
    {
        public bool IsSuccess => false;

        public string Message { get; }
        public ErrorStatuses Status { get; } 
        public Fail(string message, ErrorStatuses status = ErrorStatuses.None)
        {
            Message = message;
            Status = status;
        }
    }
}
