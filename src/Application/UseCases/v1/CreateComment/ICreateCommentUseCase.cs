using Application.UseCases.v1.CreateComment.Models;

namespace Application.UseCases.v1.CreateComment
{
    public interface ICreateCommentUseCase
    {
        Task<CreateCommentOutput> CreateCommentAsync(CreateCommentInput input);
    }
}
