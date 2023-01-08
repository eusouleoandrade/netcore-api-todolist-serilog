using AutoMapper;
using Core.Application.Dtos.Requests;
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
    public class SetDoneTodoUseCaseTest
    {
        private readonly Mock<IGenericRepositoryAsync<Todo, int>> _genericRepositoryAsyncMock;
        private readonly Mock<IGetTodoUseCase> _getTodoUseCaseMock;

        public SetDoneTodoUseCaseTest()
        {
            // Repository mock
            _genericRepositoryAsyncMock = new Mock<IGenericRepositoryAsync<Todo, int>>();

            // UseCase mock
            _getTodoUseCaseMock = new Mock<IGetTodoUseCase>();
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

            var updateGenericRepositoryAsyncResponse = true;
            _genericRepositoryAsyncMock.Setup(x => x.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(updateGenericRepositoryAsyncResponse);

            var setDoneTodoUseCaseRequest = new SetDoneTodoUseCaseRequest(id, done);

            var setDoneTodoUseCase = new SetDoneTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            // Act
            var setDoneTodoUseCaseResponse = await setDoneTodoUseCase.RunAsync(setDoneTodoUseCaseRequest);

            // Assert
            setDoneTodoUseCaseResponse.Should().BeTrue();

            setDoneTodoUseCase.HasErrorNotification.Should().BeFalse();

            setDoneTodoUseCase.ErrorNotifications.Should().HaveCount(0);
            setDoneTodoUseCase.ErrorNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Should not execute successfully when failed to update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        [Theory(DisplayName = "Should not execute successfully when failed to update")]
        [InlineData(1, "Ir ao mercado.", true)]
        [InlineData(2, "Ir ao Dentista.", false)]
        [InlineData(3, "Fazer investimentos.", true)]
        [InlineData(4, "Pagar as contas.", false)]
        public async Task ShouldNotExecute_WhenFailedToUpdate(int id, string title, bool done)
        {
            // Arranje
            var getTodoUseCaseResponse = new GetTodoUseCaseResponse(id, title, done);
            _getTodoUseCaseMock.Setup(x => x.RunAsync(It.IsAny<int>())).ReturnsAsync(getTodoUseCaseResponse);

            var updateGenericRepositoryAsyncResponse = false;
            _genericRepositoryAsyncMock.Setup(x => x.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(updateGenericRepositoryAsyncResponse);

            var setDoneTodoUseCaseRequest = new SetDoneTodoUseCaseRequest(id, done);

            var setDoneTodoUseCase = new SetDoneTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            // Act
            var setDoneTodoUseCaseResponse =  await setDoneTodoUseCase.RunAsync(setDoneTodoUseCaseRequest);

            // Assert
            setDoneTodoUseCaseResponse.Should().Be(default);

            setDoneTodoUseCase.HasErrorNotification.Should().BeTrue();

            setDoneTodoUseCase.ErrorNotifications.Should().HaveCount(1);
            setDoneTodoUseCase.ErrorNotifications.Should().NotBeEmpty();
            setDoneTodoUseCase.ErrorNotifications.Should().ContainSingle();
            setDoneTodoUseCase.ErrorNotifications.Should().Satisfy(e => e.Key == "COD0006" && e.Message == "Failed to update Todo.");

            setDoneTodoUseCase.SuccessNotifications.Should().BeEmpty();
        }
    }
}