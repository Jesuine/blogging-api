using Application.UseCases.v1.GetAllBlogPosts.Models;
using Domain.Entities;

namespace Application.UseCases.v1.GetAllBlogPosts.Mappers
{
    public static class GetAllBlogPostsOutputMapper
    {
        public static GetAllBlogPostsOutput MapToGetAllBlogPostsOutput(this IEnumerable<BlogPost> blogPosts) =>
            new GetAllBlogPostsOutput
            {
                BlogPosts = blogPosts.Select(posts => new GetBlogPostOutput()
                {
                    Content = posts.Content,
                    Id = posts.Id,
                    NumberOfComments = posts.Comments.Count(),
                    Title = posts.Title
                })
            };
        
    }
}
