namespace Domain.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; }

        //include text size validation
    }
}
