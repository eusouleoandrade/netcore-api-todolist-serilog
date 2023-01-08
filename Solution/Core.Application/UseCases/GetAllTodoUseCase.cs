using AutoMapper;
using Core.Application.Dtos.Queries;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Domain.Entities;

namespace Core.Application.UseCases
{
    public class GetAllTodoUseCase : IGetAllTodoUseCase
    {
        private readonly IGenericRepositoryAsync<Todo, int> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetAllTodoUseCase(IGenericRepositoryAsync<Todo, int> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TodoQuery>> RunAsync()
        {
            var entities = await _genericRepositoryAsync.GetAllAsync();

            return _mapper.Map<IReadOnlyList<TodoQuery>>(entities);
        }
    }
}