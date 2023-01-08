using AutoMapper;
using Core.Application.Interfaces.Repositories;
using Core.Application.Mappings;
using Core.Application.UseCases;
using Core.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.Unit.Application.UseCases
{
    public class GetAllTodoUseCaseTest
    {
        private readonly IMapper _mapperMock;
        private readonly Mock<IGenericRepositoryAsync<Todo, int>> _genericRepositoryAsyncMock;

        public GetAllTodoUseCaseTest()
        {
            // Repository mock
            _genericRepositoryAsyncMock = new Mock<IGenericRepositoryAsync<Todo, int>>();

            // Set auto mapper configs
            var mapperConfigurationMock = new MapperConfiguration(cfg => cfg.AddProfile(new GeneralProfile()));
            _mapperMock = mapperConfigurationMock.CreateMapper();
        }

        /// <summary>
        /// Should execute successfully
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Should execute successfully")]
        public async Task ShouldExecuteSucessfully()
        {
            // Arranje
            var todos = new List<Todo>()
            {
                new Todo(1, "Ir ao mercado.", false),
                new Todo(2, "Fazer investimentos.", true),
                new Todo(3, "Fazer atividade física.", false),
                new Todo(4, "Pagar as contas do mês.", true)
            };

            _genericRepositoryAsyncMock.Setup(x => x.GetAllAsync()).ReturnsAsync(todos);
            var getAllTodoUseCase = new GetAllTodoUseCase(_genericRepositoryAsyncMock.Object, _mapperMock);

            // Act
            var todoQueryList = await getAllTodoUseCase.RunAsync();

            // Assert
            todoQueryList.Should().NotBeNullOrEmpty();
            todoQueryList.Should().BeEquivalentTo(todos);
            todoQueryList.Should().HaveCount(4);

            todoQueryList.Should().Satisfy(
                e => e.Id == 1 && e.Title == "Ir ao mercado." && !e.Done,
                e => e.Id == 2 && e.Title == "Fazer investimentos." && e.Done,
                e => e.Id == 3 && e.Title == "Fazer atividade física." && !e.Done,
                e => e.Id == 4 && e.Title == "Pagar as contas do mês." && e.Done);
        }
    }
}