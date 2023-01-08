using AutoMapper;
using Core.Application.Dtos.Responses;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Application.Resources;
using Core.Domain.Entities;
using Infra.Notification.Abstractions;
using Infra.Notification.Extensions;

namespace Core.Application.UseCases
{
    public class GetTodoUseCase : Notifiable, IGetTodoUseCase
    {
        private readonly IGenericRepositoryAsync<Todo, int> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetTodoUseCase(IGenericRepositoryAsync<Todo, int> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<GetTodoUseCaseResponse?> RunAsync(int id)
        {
            Validade(id);

            if (HasErrorNotification)
                return default;

            var todo = await _genericRepositoryAsync.GetAsync(id);

            if (todo is null)
            {
                AddErrorNotification(Msg.DATA_OF_X0_X1_NOT_FOUND_COD, Msg.DATA_OF_X0_X1_NOT_FOUND_TXT.ToFormat("Todo", id));
                return default;
            }

            var useCaseResponse = _mapper.Map<GetTodoUseCaseResponse>(todo);

            return useCaseResponse;
        }

        private void Validade(int id)
        {
            if (id <= Decimal.Zero)
                AddErrorNotification(Msg.IDENTIFIER_X0_IS_INVALID_COD, Msg.IDENTIFIER_X0_IS_INVALID_TXT.ToFormat(id));
        }
    }
}