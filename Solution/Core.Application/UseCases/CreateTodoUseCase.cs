using AutoMapper;
using Core.Application.Dtos.Requests;
using Core.Application.Dtos.Responses;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Domain.Entities;
using Infra.Notification.Abstractions;

namespace Core.Application.UseCases
{
    public class CreateTodoUseCase : Notifiable, ICreateTodoUseCase
    {
        private readonly IMapper _mapper;
        private readonly ITodoRepositoryAsync _todoRepositoryAsync;

        public CreateTodoUseCase(IMapper mapper, ITodoRepositoryAsync todoRepositoryAsync)
        {
            _mapper = mapper;
            _todoRepositoryAsync = todoRepositoryAsync;
        }

        public async Task<CreateTodoUseCaseResponse?> RunAsync(CreateTodoUseCaseRequest request)
        {
            if (request.HasErrorNotification)
            {
                AddErrorNotifications(request);
                return default;
            }

            var todo = _mapper.Map<Todo>(request);

            var todoRepositoryResponse = await _todoRepositoryAsync.CreateAsync(todo);

            return _mapper.Map<CreateTodoUseCaseResponse>(todoRepositoryResponse);
        }
    }
}