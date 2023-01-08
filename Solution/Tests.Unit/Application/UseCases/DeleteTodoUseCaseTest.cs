using AutoMapper;
using Core.Application.Dtos.Responses;
using Core.Application.Interfaces.Repositories;
using Core.Application.Interfaces.UseCases;
using Core.Application.Mappings;
using Core.Application.UseCases;
using Core.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.Unit.Application.UseCases
{
    public class DeleteTodoUseCaseTest
    {
        private readonly IMapper _mapperMock;
        private readonly Mock<IGenericRepositoryAsync<Todo, int>> _genericRepositoryAsyncMock;
        private readonly Mock<IGetTodoUseCase> _getTodoUseCaseMock;

        public DeleteTodoUseCaseTest()
        {
            // Repository mock
            _genericRepositoryAsyncMock = new Mock<IGenericRepositoryAsync<Todo, int>>();

            // UseCase mock
            _getTodoUseCaseMock = new Mock<IGetTodoUseCase>();

            // Set auto mapper configs
            var mapperConfigurationMock = new MapperConfiguration(cfg => cfg.AddProfile(new GeneralProfile()));
            _mapperMock = mapperConfigurationMock.CreateMapper();
        }

        /// <summary>
        /// Should execute successfully
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        [Theory(DisplayName = "Should execute successfully")]
        [InlineData(1, "Ir ao mercado.", true)]
        [InlineData(2, "Ir ao Dentista.", false)]
        [InlineData(3, "Fazer investimentos.", true)]
        [InlineData(4, "Pagar as contas.", false)]
        public async Task ShouldExecuteSucessfully(int id, string title, bool done)
        {
            // Arranje
            var getTodoUseCaseResponse = new GetTodoUseCaseResponse(id, title, done);
            _getTodoUseCaseMock.Setup(x => x.RunAsync(It.IsAny<int>())).ReturnsAsync(getTodoUseCaseResponse);

            var deleteGenericRepositoryAsyncResponse = true;
            _genericRepositoryAsyncMock.Setup(x => x.DeleteAsync(It.IsAny<Todo>())).ReturnsAsync(deleteGenericRepositoryAsyncResponse);

            var deleteTodoUseCase = new DeleteTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object, _mapperMock);

            // Act
            var deleteUseCaseResponse = await deleteTodoUseCase.RunAsync(id);

            // Assert
            deleteUseCaseResponse.Should().BeTrue();

            deleteTodoUseCase.HasErrorNotification.Should().BeFalse();
            deleteTodoUseCase.ErrorNotifications.Should().HaveCount(0);
            deleteTodoUseCase.ErrorNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Should not execute successfully when failed to remove
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        [InlineData(1, "Ir ao mercado.", true)]
        [InlineData(2, "Ir ao Dentista.", false)]
        [InlineData(3, "Fazer investimentos.", true)]
        [InlineData(4, "Pagar as contas.", false)]
        [Theory(DisplayName = "Should not execute successfully when failed to remove")]
        public async Task ShouldNotExecute_WhenFailedToRemove(int id, string title, bool done)
        {
            // Arranje
            var getTodoUseCaseResponse = new GetTodoUseCaseResponse(id, title, done);
            _getTodoUseCaseMock.Setup(x => x.RunAsync(It.IsAny<int>())).ReturnsAsync(getTodoUseCaseResponse);

            var deleteGenericRepositoryAsyncResponse = false;
            _genericRepositoryAsyncMock.Setup(x => x.DeleteAsync(It.IsAny<Todo>())).ReturnsAsync(deleteGenericRepositoryAsyncResponse);

            var deleteTodoUseCase = new DeleteTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object, _mapperMock);

            // Act
            var deleteUseCaseResponse = await deleteTodoUseCase.RunAsync(id);

            // Assert
            deleteUseCaseResponse.Should().Be(default);

            deleteTodoUseCase.HasErrorNotification.Should().BeTrue();

            deleteTodoUseCase.ErrorNotifications.Should().HaveCount(1);
            deleteTodoUseCase.ErrorNotifications.Should().NotBeEmpty();
            deleteTodoUseCase.ErrorNotifications.Should().ContainSingle();
            deleteTodoUseCase.ErrorNotifications.Should().Satisfy(e => e.Key == "COD0003" && e.Message == "Failed to remove Todo.");

            deleteTodoUseCase.SuccessNotifications.Should().BeEmpty();
        }
    }
}