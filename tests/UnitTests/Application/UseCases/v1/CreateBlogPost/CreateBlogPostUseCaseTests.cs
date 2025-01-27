using Application.Interfaces;
using Application.UseCases.v1.CreateBlogPost;
using Application.UseCases.v1.CreateBlogPost.Models;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.v1.CreateBlogPost
{
    public class CreateBlogPostUseCaseTests
    {
        private readonly Mock<ILogger<CreateBlogPostUseCase>> _loggerMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IValidator<CreateBlogPostsInput>> _validatorMock;

        public CreateBlogPostUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<CreateBlogPostUseCase>>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _validatorMock = new Mock<IValidator<CreateBlogPostsInput>>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Input_Is_Invalid()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "", Content: "", Id: Guid.Empty);

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult
                          {
                              Errors = { new FluentValidation.Results.ValidationFailure("Title", "Title is required") }
                          });

            var useCase = new CreateBlogPostUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateBlogPosts(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("Invalid input for create a BlogPost");
        }

        [Fact]
        public async Task Should_Return_Error_When_Repository_Fails_To_Insert()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "Valid Title", Content: "Valid Content", Id: new Guid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _postRepositoryMock.Setup(repo => repo.AddBlogPostAsync(It.IsAny<BlogPost>()))
                          .ReturnsAsync(Guid.Empty);

            var useCase = new CreateBlogPostUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateBlogPosts(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("Fail on insert BlogPost");
        }

        [Fact]
        public async Task Should_Return_Success_When_BlogPost_Is_Created()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "Valid Title", Content: "Valid Content", Id: new Guid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var blogPostId = Guid.NewGuid();
            _postRepositoryMock.Setup(repo => repo.AddBlogPostAsync(It.IsAny<BlogPost>()))
                          .ReturnsAsync(blogPostId);

            var useCase = new CreateBlogPostUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act
            var result = await useCase.CreateBlogPosts(input);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Id.Should().Be(blogPostId);
        }

        [Fact]
        public async Task Should_Log_Error_When_Exception_Is_Thrown()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "Valid Title", Content: "Valid Content", Id: new Guid());

            _validatorMock.Setup(v => v.ValidateAsync(input, default))
                          .ThrowsAsync(new Exception("Validation Exception"));

            var useCase = new CreateBlogPostUseCase(_loggerMock.Object, _postRepositoryMock.Object, _validatorMock.Object);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<Exception>(() => useCase.CreateBlogPosts(input));

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
