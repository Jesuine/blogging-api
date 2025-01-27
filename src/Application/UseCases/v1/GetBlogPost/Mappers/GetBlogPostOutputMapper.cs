using Application.UseCases.v1.GetBlogPost.Models;
using Domain.Entities;

namespace Application.UseCases.v1.GetBlogPost.Mappers
{
    public static class GetBlogPostOutputMapper
    {
        public static GetBlogPostOutput MapToGetBlogPostOutput(this BlogPost blogPost) =>
            new GetBlogPostOutput
            {
                Content = blogPost.Content,
                Id = blogPost.Id,
                Comments = blogPost.Comments?.Any() is true ? 
                                blogPost.Comments.Select(comment => new CommentOutput() 
                                    {
                                        Content = comment.Content,
                                        Id = comment.Id,
                                        CreatedAt = comment.CreatedAt
                                    }) 
                               : new List<CommentOutput>(),
                Title = blogPost.Title,
                CreatedAt = blogPost.CreatedAt
            };

    }
}
