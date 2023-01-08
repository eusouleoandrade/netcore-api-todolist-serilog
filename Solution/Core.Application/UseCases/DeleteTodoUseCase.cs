using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Application.Resources;
using Core.Domain.Entities;
using Infra.Notification.Abstractions;
using Infra.Notification.Extensions;

namespace Core.Application.UseCases
{
    public class DeleteTodoUseCase : Notifiable, IDeleteTodoUseCase
    {
        private readonly IGenericRepositoryAsync<Todo, int> _genericRepositoryAsync;
        private readonly IGetTodoUseCase _getTodoUseCase;
        private readonly IMapper _mapper;

        public DeleteTodoUseCase(IGenericRepositoryAsync<Todo, int> genericRepositoryAsync, IGetTodoUseCase getTodoUseCase, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _getTodoUseCase = getTodoUseCase;
            _mapper = mapper;
        }

        public async Task<bool> RunAsync(int id)
        {
            var getTodoUseCaseResponse = await _getTodoUseCase.RunAsync(id);

            if (_getTodoUseCase.HasErrorNotification)
            {
                AddErrorNotifications(_getTodoUseCase);
                return default;
            }

            var todo = _mapper.Map<Todo>(getTodoUseCaseResponse);

            var removed = await _genericRepositoryAsync.DeleteAsync(todo);

            if (!removed)
                AddErrorNotification(Msg.FAILED_TO_REMOVE_X0_COD, Msg.FAILED_TO_REMOVE_X0_TXT.ToFormat("Todo"));

            return removed;
        }
    }
}