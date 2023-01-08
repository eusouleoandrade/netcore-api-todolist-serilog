using Core.Application.Dtos.Requests;
using Infra.Notification.Interfaces;

namespace Core.Application.Interfaces.UseCases
{
    public interface IUpdateTodoUseCase : INotifiable, IUseCase<UpdateTodoUseCaseRequest, bool>
    {
    }
}
