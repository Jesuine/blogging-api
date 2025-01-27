using Application.UseCases.v1.CreateComment.Models;
using Application.UseCases.v1.GetBlogPost.Models;
using Application.UseCases.v1.GetBlogPost.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests.Application.UseCases.v1.GetBlogPost.Validators
{
    public class GetBlogPostInputValidatorTests
    {
        private readonly GetBlogPostInputValidator _validator;

        public GetBlogPostInputValidatorTests()
        {
            _validator = new GetBlogPostInputValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var input = new GetBlogPostInput(Id: Guid.Empty);

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Id can't be empty");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var input = new GetBlogPostInput( Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
