using Application.Interfaces;
using Application.UseCases.v1.CreateComment;
using Application.UseCases.v1.CreateComment.Models;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.v1.CreateComment
{
    public class CreateCommentUseCaseTests
    {
        private readonly Mock<ILogger<CreateCommentUseCase>> _loggerMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IValidator<CreateCommentInput>> _validatorMock;
        public CreateCommentUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<CreateCommentUseCase>>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _validatorMock = new Mock<IValidator<CreateCommentInput>>();
        }


        [Fact]
        public async Task Should_Return_Error_When_Input_Is_Invalid()
        {
            // Arrange
            var input = new CreateCommentInput(Content: "", BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Content", "Content is required") } });

            var useCase = new CreateCommentUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateCommentAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == "Invalid input for create a comment");
        }

        [Fact]
        public async Task Should_Return_Error_When_Repository_Fails_To_Insert()
        {
            // Arrange
            var input = new CreateCommentInput ( Content : "Valid Content", BlogPostId : Guid.NewGuid(), Id: Guid.NewGuid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult());

            _postRepositoryMock.Setup(repo => repo.AddCommentAsync(It.IsAny<Comment>()))
                          .ReturnsAsync(Guid.Empty);

            var useCase = new CreateCommentUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateCommentAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == "Fail on insert Comment");
        }

        [Fact]
        public async Task Should_Return_Success_When_Comment_Is_Created()
        {
            // Arrange
            var input = new CreateCommentInput (Content: "Valid Content", BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult());

            var commentId = Guid.NewGuid();
            _postRepositoryMock.Setup(repo => repo.AddCommentAsync(It.IsAny<Comment>()))
                          .ReturnsAsync(commentId);

            var useCase = new CreateCommentUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateCommentAsync(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Id.Should().Be(commentId);
        }

        [Fact]
        public async Task Should_Log_Error_When_Exception_Is_Thrown()
        {
            // Arrange
            var input = new CreateCommentInput (Content: "Valid Content", BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ThrowsAsync(new Exception("Validation Exception"));

            var useCase = new CreateCommentUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            Func<Task> act = async () => await useCase.CreateCommentAsync(input);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Validation Exception");

            _loggerMock.Verify(
              x => x.Log(
                  LogLevel.Error,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error on try to execute")),
                  It.IsAny<Exception>(),
                  It.IsAny<Func<It.IsAnyType, Exception, string>>()
              ),
              Times.Once
          );
        }
    }
}
