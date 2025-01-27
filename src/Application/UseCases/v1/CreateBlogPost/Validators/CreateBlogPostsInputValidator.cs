using Application.UseCases.v1.CreateBlogPost.Models;
using FluentValidation;

namespace Application.UseCases.v1.CreateBlogPost.Validators
{
    public class CreateBlogPostsInputValidator : AbstractValidator<CreateBlogPostsInput>
    {
        public CreateBlogPostsInputValidator()
        {
             RuleFor(input => input.Title)
                .NotNull().WithMessage("Title can't be null")
                .NotEmpty().WithMessage("Title can't be empty");

            RuleFor(input => input.Content)
                .NotNull().WithMessage("Content can't be null")
                .NotEmpty().WithMessage("Content can't be empty");

            RuleFor(input => input.Id)
                .NotNull().WithMessage("Id can't be null")
                .NotEmpty().WithMessage("Id can't be empty");
        }
    }
}
