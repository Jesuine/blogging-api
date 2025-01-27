using Application.Interfaces;
using Application.UseCases.v1.GetBlogPost.Mappers;
using Application.UseCases.v1.GetBlogPost.Models;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.v1.GetBlogPost
{
    public class GetBlogPostUseCase : IGetBlogPostUseCase
    {
        private ILogger<GetBlogPostUseCase> _logger;
        private IPostRepository _postRepository;
        private IValidator<GetBlogPostInput> _validator;

        public GetBlogPostUseCase(
            ILogger<GetBlogPostUseCase> logger, 
            IPostRepository postRepository,
            IValidator<GetBlogPostInput> validator)
        {
            _logger = logger;
            _postRepository = postRepository;
            _validator = validator;
        }

        public async Task<GetBlogPostOutput> GetBlogPostById(GetBlogPostInput getBlogPostInput)
        {
            var output = new GetBlogPostOutput();
            try
            {
                _logger.LogInformation("[{useCase}] - Starting {method}", nameof(GetBlogPostUseCase), nameof(GetBlogPostById));

                var validationResult = await _validator.ValidateAsync(getBlogPostInput);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{useCase}] - Invalid input for method {method}", nameof(GetBlogPostUseCase), nameof(GetBlogPostById));
                    output.AddError("Invalid input for GetBlogPostById");

                    return output;
                }

                BlogPost blogPost = await _postRepository.GetBlogPostWithCommentByIdAsync(getBlogPostInput.Id);

                if (blogPost is null) 
                { 
                    _logger.LogWarning("[{useCase}] - No blogPost found for Id {id} - method {method}", nameof(GetBlogPostUseCase), getBlogPostInput.Id, nameof(GetBlogPostById));
                    output.AddError($"No blogPost found for id {getBlogPostInput.Id}");
                    return output;
                }

                output = blogPost.MapToGetBlogPostOutput();

                _logger.LogInformation("[{useCase}] - Finishing {method}", nameof(GetBlogPostUseCase), nameof(GetBlogPostById));
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "[{useCase}] - Error on try to execute {method}", nameof(GetBlogPostUseCase), nameof(GetAllBlogPosts));
                throw;
            }

            return output;
        }
    }
}
