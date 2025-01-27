using Application.Interfaces;
using Application.UseCases.v1.CreateComment;
using Application.UseCases.v1.GetAllBlogPosts;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.v1.GetAllBlogPosts
{
    public class GetAllBlogPostsUseCaseTests
    {
        private readonly Mock<ILogger<GetAllBlogPostsUseCase>> _loggerMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;

        public GetAllBlogPostsUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<GetAllBlogPostsUseCase>>();
            _postRepositoryMock = new Mock<IPostRepository>();
        }

        [Fact]
        public async Task Should_Return_Error_When_No_BlogPosts_Found()
        {
            // Arrange
            _postRepositoryMock.Setup(repo => repo.GetAllBlogPostAsync())
                           .ReturnsAsync(new List<BlogPost>());

            var useCase = new GetAllBlogPostsUseCase(_loggerMock.Object, _postRepositoryMock.Object);

            // Act
            var result = await useCase.GetAllBlogPosts();

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e == " No blogPosts found");
        }

        [Fact]
        public async Task Should_Return_BlogPosts_When_Found()
        {
            // Arrange
            var blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = Guid.NewGuid(), Title = "Post 1", Content = "Content 1", CreatedAt = DateTime.UtcNow, Comments = new List<Comment>() },
                new BlogPost { Id = Guid.NewGuid(), Title = "Post 2", Content = "Content 2", CreatedAt = DateTime.UtcNow, Comments = new List<Comment>() }
            };

            _postRepositoryMock.Setup(repo => repo.GetAllBlogPostAsync())
                           .ReturnsAsync(blogPosts);

            var useCase = new GetAllBlogPostsUseCase(_loggerMock.Object, _postRepositoryMock.Object);

            // Act
            var result = await useCase.GetAllBlogPosts();

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.BlogPosts.Should().HaveCount(2);
            result.BlogPosts.Select(bp => bp.Title).Should().Contain(new[] { "Post 1", "Post 2" });
        }

        [Fact]
        public async Task Should_Log_Error_When_Exception_Is_Thrown()
        {
            // Arrange
            _postRepositoryMock.Setup(repo => repo.GetAllBlogPostAsync())
                           .ThrowsAsync(new Exception("Database Error"));

            var useCase = new GetAllBlogPostsUseCase(_loggerMock.Object, _postRepositoryMock.Object);

            // Act
            Func<Task> act = async () => await useCase.GetAllBlogPosts();

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database Error");

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
