using System;
using Xunit;
using Core.Application.UseCases;
using Core.Application.Interfaces;
using Moq;
using Core.Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Core.Application.Interfaces.UseCases;
using Core.Application.Dtos.Requests;
using Core.Application.Dtos.Responses;
using FluentAssertions;

namespace Tests.Unit.Application.UseCases
{
	public class UpdateTodoUseCaseTest
	{
        private readonly Mock<IGenericRepositoryAsync<Todo, int>> _genericRepositoryAsyncMock;
        private readonly Mock<IGetTodoUseCase> _getTodoUseCaseMock;

		public UpdateTodoUseCaseTest()
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
        public async Task ShouldExecuteSuccessfully(int id, string title, bool done)
        {
            // Arranje
            var getTodoUseCaseResponse = new GetTodoUseCaseResponse(id, title, done);
            _getTodoUseCaseMock.Setup(x => x.RunAsync(It.IsAny<int>())).ReturnsAsync(getTodoUseCaseResponse);

            var updateGenericRepositoryAsyncResponse = true;
            _genericRepositoryAsyncMock.Setup(x => x.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(updateGenericRepositoryAsyncResponse);

            var updateTodoUseCase = new UpdateTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            var updateTodoUseCaseRequest = new UpdateTodoUseCaseRequest(id, $"{title} updated", !done);

            // Act
            var updateTodoUseCaseResponse = await updateTodoUseCase.RunAsync(updateTodoUseCaseRequest);

            // Assert
            updateTodoUseCaseResponse.Should().BeTrue();

            updateTodoUseCase.HasErrorNotification.Should().BeFalse();

            updateTodoUseCase.ErrorNotifications.Should().HaveCount(0);
            updateTodoUseCase.ErrorNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Should not execute when id is invalid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Theory(DisplayName = "Should not execute when id is invalid")]
        [InlineData(0, "Ir ao mercado.", true)]
        [InlineData(-1, "Ir ao Dentista.", false)]
        [InlineData(-2, "Fazer investimentos.", true)]
        [InlineData(-3, "Pagar as contas.", false)]
        public async Task ShouldNotExecute_WhenIdIsInvalid(int id, string title, bool done)
        {
            // Arranje
            var updateTodoUseCase = new UpdateTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            var updateTodoUseCaseRequest = new UpdateTodoUseCaseRequest(id, title, done);

            // Act
            var updateTodoUseCaseResponse = await updateTodoUseCase.RunAsync(updateTodoUseCaseRequest);

            // Assert
            updateTodoUseCaseResponse.Should().Be(default);

            updateTodoUseCase.HasErrorNotification.Should().BeTrue();

            updateTodoUseCase.ErrorNotifications.Should().NotBeEmpty();
            updateTodoUseCase.ErrorNotifications.Should().HaveCount(1);
            updateTodoUseCase.ErrorNotifications.Should().ContainSingle();
            updateTodoUseCase.ErrorNotifications.Should().Satisfy(e => e.Key == "COD0005" && e.Message == $"Identifier {id} is invalid.");

            updateTodoUseCase.SuccessNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Should not execute when title is invalid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        [Theory(DisplayName = "Should not execute when title is invalid")]
        [InlineData(1, " ", true)]
        [InlineData(2, "", true)]
        public async Task ShouldNotExecute_WhenTitleIsInvalid(int id, string title, bool done)
        {
            // Arranje
            var updateTodoUseCase = new UpdateTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            var updateTodoUseCaseRequest = new UpdateTodoUseCaseRequest(id, title, done);

            // Act
            var updateTodoUseCaseResponse = await updateTodoUseCase.RunAsync(updateTodoUseCaseRequest);

            // Assert
            updateTodoUseCaseResponse.Should().Be(default);

            updateTodoUseCase.HasErrorNotification.Should().BeTrue();

            updateTodoUseCase.ErrorNotifications.Should().NotBeEmpty();
            updateTodoUseCase.ErrorNotifications.Should().HaveCount(1);
            updateTodoUseCase.ErrorNotifications.Should().ContainSingle();
            updateTodoUseCase.ErrorNotifications.Should().Satisfy(e => e.Key == "COD0001" && e.Message == "Title is required.");

            updateTodoUseCase.SuccessNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Should not execute when failed to update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        [Theory(DisplayName = "Should not execute when failed to update")]
        [InlineData(1, "Ir ao mercado.", true)]
        [InlineData(2, "Ir ao Dentista.", false)]
        [InlineData(3, "Fazer investimentos.", true)]
        [InlineData(4, "Pagar as contas.", false)]
        public async Task ShouldNotExecute_WhenFailedToUpdate(int id, string title, bool done)
        {
            // Arranje
            var updateGenericRepositoryAsyncResponse = false;
            _genericRepositoryAsyncMock.Setup(x => x.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(updateGenericRepositoryAsyncResponse);

            var getTodoUseCaseResponse = new GetTodoUseCaseResponse(id, title, done);
            _getTodoUseCaseMock.Setup(x => x.RunAsync(It.IsAny<int>())).ReturnsAsync(getTodoUseCaseResponse);

            var updateTodoUseCase = new UpdateTodoUseCase(_genericRepositoryAsyncMock.Object, _getTodoUseCaseMock.Object);

            var updateTodoUseCaseRequest = new UpdateTodoUseCaseRequest(id, $"{title} updated", !done);

            // Act
            var updateTodoUseCaseResponse = await updateTodoUseCase.RunAsync(updateTodoUseCaseRequest);

            // Assert
            updateTodoUseCaseResponse.Should().Be(default);

            updateTodoUseCase.HasErrorNotification.Should().BeTrue();

            updateTodoUseCase.ErrorNotifications.Should().NotBeEmpty();
            updateTodoUseCase.ErrorNotifications.Should().HaveCount(1);
            updateTodoUseCase.ErrorNotifications.Should().ContainSingle();
            updateTodoUseCase.ErrorNotifications.Should().Satisfy(e => e.Key == "COD0006" && e.Message == "Failed to update Todo.");

            updateTodoUseCase.SuccessNotifications.Should().BeEmpty();
        }
    }
}