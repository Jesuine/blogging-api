using Application.UseCases.v1.CreateComment.Models;
using Application.UseCases.v1.CreateComment.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests.Application.UseCases.v1.CreateComment.Validators
{
    public class CreateCommentInputValidatorTests
    {
        private readonly CreateCommentInputValidator _validator;

        public CreateCommentInputValidatorTests()
        {
            _validator = new CreateCommentInputValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Content_Is_Null()
        {
            // Arrange
            var input = new CreateCommentInput(Content: null, BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                  .WithErrorMessage("Title can't be null");
        }

        [Fact]
        public void Should_Have_Error_When_Content_Is_Empty()
        {
            // Arrange
            var input = new CreateCommentInput(Content: "", BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                  .WithErrorMessage("Title can't be empty");
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var input = new CreateCommentInput(Content: "Valid Content", BlogPostId: Guid.NewGuid(), Id: Guid.Empty);

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Id can't be empty");
        }

        [Fact]
        public void Should_Have_Error_When_BlogPostId_Is_Empty()
        {
            // Arrange
            var input = new CreateCommentInput (Content: "Valid Content", BlogPostId: Guid.Empty, Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BlogPostId)
                  .WithErrorMessage("Id can't be empty");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var input = new CreateCommentInput (Content: "Valid Content", BlogPostId: Guid.NewGuid(), Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
