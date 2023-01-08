namespace Core.Application.Interfaces.UseCases
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse?> RunAsync(TRequest request);
    }

    public interface IUseCase<TRequest>
    {
        Task RunAsync(TRequest request);
    }
}