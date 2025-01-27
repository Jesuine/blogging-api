using Application.Common.Outputs;

namespace Application.UseCases.v1.CreateComment.Models
{
    public class CreateCommentOutput : BaseOutput
    {
        public Guid Id { get; set; }
    }
}
