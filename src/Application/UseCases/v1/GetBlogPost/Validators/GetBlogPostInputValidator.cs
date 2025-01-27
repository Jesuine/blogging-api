using Application.UseCases.v1.GetBlogPost.Models;
using FluentValidation;

namespace Application.UseCases.v1.GetBlogPost.Validators
{
    public class GetBlogPostInputValidator : AbstractValidator<GetBlogPostInput>
    {
        public GetBlogPostInputValidator()
        {
            RuleFor(input => input.Id)
               .NotNull().WithMessage("Id can't be null")
               .NotEmpty().WithMessage("Id can't be empty");
        }
    }
}
