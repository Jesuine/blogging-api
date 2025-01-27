using Dapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data.Connection;
using Infrastructure.Data.Repositories;
using Xunit;

namespace IntegratedTests.Infrastructure.Data
{
    public class PostRepositoryTests : IDisposable
    {
        private readonly SqlConnectionFactory _connectionFactory;
        private readonly PostRepository _repository;

        public PostRepositoryTests()
        {
            var connectionString = "Server=localhost,1433;Database=BLOG_DATABASE;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True";
            _connectionFactory = new SqlConnectionFactory(connectionString);

            _repository = new PostRepository(_connectionFactory);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            connection.Execute("DELETE FROM Comment");
            connection.Execute("DELETE FROM BlogPost");


            var blogPostId = Guid.NewGuid();
            connection.Execute("INSERT INTO BlogPost (Id, Title, Content, CreatedAt) VALUES (@Id, @Title, @Content, @CreatedAt)",
                new { Id = blogPostId, Title = "Test Blog Post", Content = "Test Content", CreatedAt = DateTime.UtcNow });

            connection.Execute("INSERT INTO Comment (Id, Content, BlogPostId, CreatedAt) VALUES (@Id, @Content, @BlogPostId, @CreatedAt)",
                new { Id = Guid.NewGuid(), Content = "Test Comment", BlogPostId = blogPostId, CreatedAt = DateTime.UtcNow });
        }

        [Fact]
        public async Task Should_Add_New_BlogPost()
        {
            // Arrange
            var newBlogPost = new BlogPost
            {
                Id = Guid.NewGuid(),
                Title = "New Blog Post",
                Content = "New Content",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _repository.AddBlogPostAsync(newBlogPost);

            // Assert
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            var savedBlogPost = connection.QuerySingleOrDefault<BlogPost>("SELECT * FROM BlogPost WHERE Id = @Id", new { newBlogPost.Id });
            savedBlogPost.Should().NotBeNull();
            savedBlogPost.Title.Should().Be("New Blog Post");
        }

        [Fact]
        public async Task Should_Retrieve_BlogPost_By_Id()
        {
            // Arrange
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            var existingPost = connection.QueryFirstOrDefault<BlogPost>("SELECT TOP 1 * FROM BlogPost");

            // Act
            var result = await _repository.GetBlogPostByIdAsync(existingPost.Id);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(existingPost.Title);
        }

        [Fact]
        public async Task Should_Retrieve_All_BlogPosts()
        {
            // Act
            var result = await _repository.GetAllBlogPostAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }


        [Fact]
        public async Task Should_Add_Comment_By_Post_Id()
        {
            // Arrange
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            var existingPost = connection.QueryFirstOrDefault<BlogPost>("SELECT TOP 1 * FROM BlogPost");

            var comment = new Comment { 
                Id = Guid.NewGuid(),
                BlogPostId = existingPost.Id,
                Content = "Comment in post for integrated test"
            };

            // Act
            var result = await _repository.AddCommentAsync(comment);

            // Assert
            result.Should().Be(comment.Id);
        }

        public void Dispose()
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            connection.Execute("DELETE FROM Comment");
            connection.Execute("DELETE FROM BlogPost");
        }
    }
}
