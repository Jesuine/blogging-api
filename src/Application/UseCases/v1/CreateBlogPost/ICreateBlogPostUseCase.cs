using Application.UseCases.v1.CreateBlogPost.Models;

namespace Application.UseCases.v1.CreateBlogPost
{
    public interface ICreateBlogPostUseCase
    {
        Task<CreateBlogPostOutput> CreateBlogPosts(CreateBlogPostsInput input);
    }
}
