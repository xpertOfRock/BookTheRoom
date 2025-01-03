namespace Application.UseCases.Abstractions
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : notnull
    {
    }
}
