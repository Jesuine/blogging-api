using Application.UseCases.v1.GetBlogPost.Models;

namespace Application.UseCases.v1.GetBlogPost
{
    public interface IGetBlogPostUseCase
    {
        Task<GetBlogPostOutput> GetBlogPostById(GetBlogPostInput getBlogPostInput);
    }
}
