using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Infrastructure.Data.Connection;


namespace Infrastructure.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PostRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<BlogPost> GetBlogPostByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = "SELECT * FROM BlogPost WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<BlogPost>(query, new { Id = id });
        }

        public async Task<BlogPost> GetBlogPostWithCommentByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                                SELECT 
                                    bp.Id , 
                                    bp.Title, 
                                    bp.Content, 
                                    bp.CreatedAt,
                                    c.Id AS CommentId, 
                                    c.Id, 
                                    c.Content, 
                                    c.BlogPostId, 
                                    c.CreatedAt
                                FROM BlogPost bp
                                LEFT JOIN Comment c ON bp.Id = c.BlogPostId
                                WHERE bp.Id = @Id;
                                ";

            var blogPostDictionary = new Dictionary<Guid, BlogPost>();

            var result = await connection.QueryAsync<BlogPost, Comment, BlogPost>(
                query,
                (blogPost, comment) =>
                {
                    if (!blogPostDictionary.TryGetValue(blogPost.Id, out var currentBlogPost))
                    {
                        currentBlogPost = new BlogPost
                        {
                            Id = blogPost.Id,
                            Title = blogPost.Title,
                            Content = blogPost.Content,
                            CreatedAt = blogPost.CreatedAt,
                            Comments = new List<Comment>()
                        };

                        blogPostDictionary.Add(currentBlogPost.Id, currentBlogPost);
                    }

                    if (comment != null && comment.Id != Guid.Empty)
                    {
                        currentBlogPost.Comments.Add(new Comment
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            BlogPostId = comment.BlogPostId,
                            CreatedAt = comment.CreatedAt
                        });
                    }

                    return currentBlogPost;
                },
                new { Id = id },
                splitOn: "CommentId"
            );

            return blogPostDictionary.Values.FirstOrDefault();
        }

        //public async Task<IEnumerable<BlogPost>> GetAllBlogPostAsync()
        //{
        //    using var connection = _connectionFactory.CreateConnection();
        //    //const string query = "SELECT * FROM BlogPost";
        //    const string query = @"        
        //                        SELECT 
        //                            bp.Id , 
        //                            bp.Title, 
        //                            bp.Content, 
        //                            bp.CreatedAt,
        //                            COUNT(c.Id) AS NumberOfComments
        //                        FROM BlogPost bp
        //                        LEFT JOIN Comment c ON bp.Id = c.BlogPostId
        //                        GROUP BY bp.Id, bp.Title, bp.Content, bp.CreatedAt
        //                    ";
        //    return await connection.QueryAsync<BlogPost>(query);
        //}

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                    SELECT 
                        bp.Id , 
                        bp.Title, 
                        bp.Content, 
                        bp.CreatedAt,
                        c.Id AS CommentId, 
                        c.Id, 
                        c.Content, 
                        c.BlogPostId, 
                        c.CreatedAt
                    FROM BlogPost bp
                    LEFT JOIN Comment c ON bp.Id = c.BlogPostId
                    ";

            var blogPostDictionary = new Dictionary<Guid, BlogPost>();

            var result = await connection.QueryAsync<BlogPost, Comment, BlogPost>(
                query,
                (blogPost, comment) =>
                {
                    if (!blogPostDictionary.TryGetValue(blogPost.Id, out var currentBlogPost))
                    {
                        currentBlogPost = new BlogPost
                        {
                            Id = blogPost.Id,
                            Title = blogPost.Title,
                            Content = blogPost.Content,
                            CreatedAt = blogPost.CreatedAt,
                            Comments = new List<Comment>()
                        };

                        blogPostDictionary.Add(currentBlogPost.Id, currentBlogPost);
                    }

                    if (comment != null && comment.Id != Guid.Empty)
                    {
                        currentBlogPost.Comments.Add(new Comment
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            BlogPostId = comment.BlogPostId,
                            CreatedAt = comment.CreatedAt
                        });
                    }

                    return currentBlogPost;
                },
                splitOn: "CommentId"
            );

            return blogPostDictionary.Values;
        }



        public async Task<Guid> AddBlogPostAsync(BlogPost post)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                INSERT INTO BlogPost (Id, Title, Content) 
                VALUES (@Id, @Title, @Content)";
            int rowsAffected = await connection.ExecuteAsync(query, post);
            return rowsAffected > 0 ? post.Id : Guid.Empty;
        }

        public async Task<Guid> AddCommentAsync(Comment comment)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                INSERT INTO Comment (Id, Content, BlogPostId) 
                VALUES (@Id, @Content, @BlogPostId)";
            int rowsAffected = await connection.ExecuteAsync(query, comment);
            return rowsAffected > 0 ? comment.Id : Guid.Empty;
        }
    }
}
