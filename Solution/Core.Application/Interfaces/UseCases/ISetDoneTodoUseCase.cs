using Core.Application.Dtos.Requests;
using Infra.Notification.Interfaces;

namespace Core.Application.Interfaces.UseCases
{
    public interface ISetDoneTodoUseCase : INotifiable, IUseCase<SetDoneTodoUseCaseRequest, bool>
    {
    }
}
