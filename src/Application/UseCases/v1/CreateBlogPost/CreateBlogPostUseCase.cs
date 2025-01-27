using Application.Interfaces;
using Application.UseCases.v1.CreateBlogPost.Mappers;
using Application.UseCases.v1.CreateBlogPost.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.v1.CreateBlogPost
{
    public class CreateBlogPostUseCase : ICreateBlogPostUseCase
    {
        private ILogger<CreateBlogPostUseCase> _logger;
        private IPostRepository _postRepository;
        private IValidator<CreateBlogPostsInput> _validator;
        public CreateBlogPostUseCase(
            ILogger<CreateBlogPostUseCase> logger, 
            IPostRepository postRepository,
            IValidator<CreateBlogPostsInput> validator)
        {
            _logger = logger;
            _postRepository = postRepository;
            _validator = validator;
        }

        public async Task<CreateBlogPostOutput> CreateBlogPosts(CreateBlogPostsInput input)
        {
            var output = new CreateBlogPostOutput();
            try
            {
                _logger.LogInformation("[{useCase}] - Starting {method}", nameof(CreateBlogPostUseCase), nameof(CreateBlogPosts));

                var validationResult = await _validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{useCase}] - Invalid input for method {method}", nameof(CreateBlogPostUseCase), nameof(CreateBlogPosts));
                    output.AddError("Invalid input for create a BlogPost");

                    return output;
                }

                output.Id = await _postRepository.AddBlogPostAsync(input.MapToBlogPost());

                if (output.Id == Guid.Empty)
                {
                    _logger.LogError("[{useCase}] - Fail on insert BlogPost - method {method}", nameof(CreateBlogPostUseCase), nameof(CreateBlogPosts));
                    output.AddError("Fail on insert BlogPost");
                    return output;
                }

                _logger.LogInformation("[{useCase}] - Finishing {method}", nameof(CreateBlogPostUseCase), nameof(CreateBlogPosts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{useCase}] - Error on try to execute {method}", nameof(CreateBlogPostUseCase), nameof(CreateBlogPosts));
                throw;
            }

            return output;
        }
    }
}
