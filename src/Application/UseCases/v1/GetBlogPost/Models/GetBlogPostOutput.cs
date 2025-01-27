using Application.Common.Outputs;

namespace Application.UseCases.v1.GetBlogPost.Models
{
    public class GetBlogPostOutput : BaseOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<CommentOutput> Comments { get; set; }
    }

    public class CommentOutput
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
