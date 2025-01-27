using Application.UseCases.v1.CreateComment.Models;
using FluentValidation;

namespace Application.UseCases.v1.CreateComment.Validators
{
    public class CreateCommentInputValidator : AbstractValidator<CreateCommentInput>
    {
        public CreateCommentInputValidator()
        {
            RuleFor(input => input.Content)
                .NotNull().WithMessage("Title can't be null")
                .NotEmpty().WithMessage("Title can't be empty");

            RuleFor(input => input.Id)
                .NotNull().WithMessage("Id can't be null")
                .NotEmpty().WithMessage("Id can't be empty");

            RuleFor(input => input.BlogPostId)
                .NotNull().WithMessage("Id can't be null")
                .NotEmpty().WithMessage("Id can't be empty");
        }
    }
}
