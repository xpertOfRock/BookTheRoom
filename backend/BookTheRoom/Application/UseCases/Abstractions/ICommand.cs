namespace Application.UseCases.Abstractions
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
