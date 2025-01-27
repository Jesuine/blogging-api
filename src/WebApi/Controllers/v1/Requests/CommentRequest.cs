using System.Text.Json.Serialization;

namespace WebApi.Controllers.v1.Requests
{
    public class CommentRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
    }
}
