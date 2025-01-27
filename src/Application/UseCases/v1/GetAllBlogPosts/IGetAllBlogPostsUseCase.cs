using Application.UseCases.v1.GetAllBlogPosts.Models;

namespace Application.UseCases.v1.GetAllBlogPosts
{
    public interface IGetAllBlogPostsUseCase
    {
        Task<GetAllBlogPostsOutput> GetAllBlogPosts();
    }
}
