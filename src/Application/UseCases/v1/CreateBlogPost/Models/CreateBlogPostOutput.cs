using Application.Common.Outputs;

namespace Application.UseCases.v1.CreateBlogPost.Models
{
    public class CreateBlogPostOutput : BaseOutput
    {
        public Guid Id { get; set; }
    }
}
