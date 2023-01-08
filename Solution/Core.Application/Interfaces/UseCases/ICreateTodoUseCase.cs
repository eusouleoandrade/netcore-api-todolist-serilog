using Core.Application.Dtos.Requests;
using Core.Application.Dtos.Responses;
using Infra.Notification.Interfaces;

namespace Core.Application.Interfaces.UseCases
{
    public interface ICreateTodoUseCase : INotifiable, IUseCase<CreateTodoUseCaseRequest, CreateTodoUseCaseResponse>
    {
    }
}
