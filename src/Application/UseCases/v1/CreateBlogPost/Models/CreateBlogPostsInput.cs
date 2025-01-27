namespace Application.UseCases.v1.CreateBlogPost.Models
{
    public record CreateBlogPostsInput(
        string Title,
        string Content,
        Guid Id
    );
}
