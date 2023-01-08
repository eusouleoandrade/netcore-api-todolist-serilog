using Core.Application.Dtos.Queries;

namespace Core.Application.Interfaces.UseCases
{
    public interface IGetAllTodoUseCase
    {
        Task<IReadOnlyList<TodoQuery>> RunAsync();
    }
}