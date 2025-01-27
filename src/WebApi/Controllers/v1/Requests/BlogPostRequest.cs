using System.Text.Json.Serialization;

namespace WebApi.Controllers.v1.Requests
{
    public class BlogPostRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
