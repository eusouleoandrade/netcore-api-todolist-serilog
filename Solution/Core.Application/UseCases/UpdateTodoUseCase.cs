using Core.Application.Dtos.Requests;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Application.Resources;
using Core.Domain.Entities;
using Infra.Notification.Abstractions;
using Infra.Notification.Extensions;

namespace Core.Application.UseCases
{
    public class UpdateTodoUseCase : Notifiable, IUpdateTodoUseCase
    {
        private readonly IGenericRepositoryAsync<Todo, int> _genericRepositoryAsync;
        private readonly IGetTodoUseCase _getTodoUseCase;

        public UpdateTodoUseCase(IGenericRepositoryAsync<Todo, int> genericRepositoryAsync, IGetTodoUseCase getTodoUseCase)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _getTodoUseCase = getTodoUseCase;
        }

        public async Task<bool> RunAsync(UpdateTodoUseCaseRequest request)
        {
            if (request.HasErrorNotification)
            {
                AddErrorNotifications(request);
                return default;
            }

            var getTodoUseCaseResponse = await _getTodoUseCase.RunAsync(request.Id);

            if (_getTodoUseCase.HasErrorNotification)
            {
                AddErrorNotifications(_getTodoUseCase);
                return default;
            }

            var todo = new Todo(getTodoUseCaseResponse.Id, request.Title, request.Done);

            var updated = await _genericRepositoryAsync.UpdateAsync(todo);

            if (!updated)
                AddErrorNotification(Msg.FAILED_TO_UPDATE_X0_COD, Msg.FAILED_TO_UPDATE_X0_TXT.ToFormat("Todo"));

            return updated;
        }
    }
}