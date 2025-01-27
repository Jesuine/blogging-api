using Application.Interfaces;
using Application.UseCases.v1.GetBlogPost;
using Application.UseCases.v1.GetBlogPost.Models;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.v1.GetBlogPost
{
    public class GetBlogPostUseCaseTests
    {
        private readonly Mock<ILogger<GetBlogPostUseCase>> _loggerMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IValidator<GetBlogPostInput>> _mockValidator;
        private readonly GetBlogPostUseCase _useCase;

        public GetBlogPostUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<GetBlogPostUseCase>>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _mockValidator = new Mock<IValidator<GetBlogPostInput>>();
            _useCase = new GetBlogPostUseCase(_loggerMock.Object, _postRepositoryMock.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Should_Return_Error_When_Input_Is_Invalid()
        {
            // Arrange
            var input = new GetBlogPostInput(Id: Guid.Empty);

            _mockValidator.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult { Errors = { new ValidationFailure("Id", "Id cannot be empty") } });

            // Act
            var result = await _useCase.GetBlogPostById(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == "Invalid input for GetBlogPostById");
        }

        [Fact]
        public async Task Should_Return_Error_When_BlogPost_Not_Found()
        {
            // Arrange
            var input = new GetBlogPostInput (Id: Guid.NewGuid());

            _mockValidator.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult());

            _postRepositoryMock.Setup(repo => repo.GetBlogPostWithCommentByIdAsync(input.Id))
                           .ReturnsAsync((BlogPost)null);

            // Act
            var result = await _useCase.GetBlogPostById(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == $"No blogPost found for id {input.Id}");
        }

        [Fact]
        public async Task Should_Return_Success_When_BlogPost_Is_Found()
        {
            // Arrange
            var input = new GetBlogPostInput (Id: Guid.NewGuid());

            _mockValidator.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new ValidationResult());

            var blogPost = new BlogPost
            {
                Id = input.Id,
                Title = "Test Blog Post",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow
            };

            _postRepositoryMock.Setup(repo => repo.GetBlogPostWithCommentByIdAsync(input.Id))
                           .ReturnsAsync(blogPost);

            // Act
            var result = await _useCase.GetBlogPostById(input);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Title.Should().Be("Test Blog Post");
            result.Content.Should().Be("Test Content");
        }

        [Fact]
        public async Task Should_Log_Error_When_Exception_Is_Thrown()
        {
            // Arrange
            var input = new GetBlogPostInput (Id: Guid.NewGuid());

            _mockValidator.Setup(v => v.ValidateAsync(input, default))
                          .ThrowsAsync(new Exception("Validation Exception"));

            // Act
            Func<Task> act = async () => await _useCase.GetBlogPostById(input);

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
