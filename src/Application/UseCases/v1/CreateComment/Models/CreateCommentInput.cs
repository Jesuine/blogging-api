namespace Application.UseCases.v1.CreateComment.Models
{
    public record CreateCommentInput(Guid Id, string Content, Guid BlogPostId);
}
