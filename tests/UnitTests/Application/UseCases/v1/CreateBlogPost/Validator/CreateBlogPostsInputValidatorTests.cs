using Application.UseCases.v1.CreateBlogPost.Models;
using Application.UseCases.v1.CreateBlogPost.Validators;
using Xunit;
using FluentValidation.TestHelper;

namespace UnitTests.Application.UseCases.v1.CreateBlogPost.Validator
{
    public class CreateBlogPostsInputValidatorTests
    {
        private readonly CreateBlogPostsInputValidator _validator;

        public CreateBlogPostsInputValidatorTests()
        {
            _validator = new CreateBlogPostsInputValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Null()
        {
            // Arrange
            var input = new CreateBlogPostsInput (Title: null, Content: "Valid Content", Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Title can't be null");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "", Content: "Valid Content", Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage("Title can't be empty");
        }

        [Fact]
        public void Should_Have_Error_When_Content_Is_Null()
        {
            // Arrange
            var input = new CreateBlogPostsInput (Title: "Valid Title", Content: null, Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                  .WithErrorMessage("Content can't be null");
        }

        [Fact]
        public void Should_Have_Error_When_Content_Is_Empty()
        {
            // Arrange
            var input = new CreateBlogPostsInput (Title: "Valid Title", Content: "", Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                  .WithErrorMessage("Content can't be empty");
        }

        [Fact]
        public void Should_Have_Error_When_Content_Id_Is_Empty()
        {
            // Arrange
            var input = new CreateBlogPostsInput(Title: "Valid Title", Content: "Valid Content", Id: Guid.Empty);

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
            var input = new CreateBlogPostsInput (Title: "Valid Title", Content: "Valid Content", Id: Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
