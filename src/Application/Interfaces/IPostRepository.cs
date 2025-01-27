using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPostRepository
    {
        Task<BlogPost> GetBlogPostByIdAsync(Guid id);

        Task<BlogPost> GetBlogPostWithCommentByIdAsync(Guid id);

        Task<IEnumerable<BlogPost>> GetAllBlogPostAsync();

        Task<Guid> AddBlogPostAsync(BlogPost post);

        Task<Guid> AddCommentAsync(Comment comment);
    }
}
