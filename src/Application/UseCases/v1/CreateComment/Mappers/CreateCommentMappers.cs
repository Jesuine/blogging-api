using Application.UseCases.v1.CreateComment.Models;
using Domain.Entities;

namespace Application.UseCases.v1.CreateComment.Mappers
{
    public static class CreateCommentMappers
    {
        public static Comment MapToComment(this CreateCommentInput input) =>
            new Comment()
            {
                Id = input.Id,
                Content = input.Content,
                BlogPostId = input.BlogPostId,
            };
    }
}
