using Application.UseCases.v1.CreateBlogPost.Models;
using Domain.Entities;

namespace Application.UseCases.v1.CreateBlogPost.Mappers
{
    public static class CreateBlobPostMappers
    {
        public static BlogPost MapToBlogPost(this CreateBlogPostsInput input) =>
            new BlogPost()
            {
                Content = input.Content,
                Id = input.Id,
                Title = input.Title
            };
    }
}
