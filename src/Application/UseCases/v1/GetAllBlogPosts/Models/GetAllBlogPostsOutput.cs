using Application.Common.Outputs;
using System.Collections.Generic;

namespace Application.UseCases.v1.GetAllBlogPosts.Models
{
    public class GetAllBlogPostsOutput : BaseOutput
    {
        public IEnumerable<GetBlogPostOutput> BlogPosts { get; set; }
    }

    public class GetBlogPostOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long NumberOfComments { get; set; }
    }
}
