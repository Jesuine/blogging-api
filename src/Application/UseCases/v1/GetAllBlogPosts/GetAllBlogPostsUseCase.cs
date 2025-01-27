using Application.Interfaces;
using Application.UseCases.v1.GetAllBlogPosts.Mappers;
using Application.UseCases.v1.GetAllBlogPosts.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.v1.GetAllBlogPosts
{
    public class GetAllBlogPostsUseCase : IGetAllBlogPostsUseCase
    {
        private ILogger<GetAllBlogPostsUseCase> _logger;
        private IPostRepository _postRepository;
        public GetAllBlogPostsUseCase(
            ILogger<GetAllBlogPostsUseCase> logger, 
            IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public async Task<GetAllBlogPostsOutput> GetAllBlogPosts()
        {
            var output = new GetAllBlogPostsOutput();
            try
            {
                _logger.LogInformation("[{useCase}] - Starting {method}", nameof(GetAllBlogPostsUseCase), nameof(GetAllBlogPosts));

                IEnumerable<BlogPost> blogPosts = await _postRepository.GetAllBlogPostAsync();

                if (!blogPosts.Any()) 
                { 
                    _logger.LogWarning("[{useCase}] - No blogPosts found for method {method}", nameof(GetAllBlogPostsUseCase), nameof(GetAllBlogPosts));
                    output.AddError(" No blogPosts found");
                    return output;
                }

                output = blogPosts.MapToGetAllBlogPostsOutput();

                _logger.LogInformation("[{useCase}] - Finishing {method}", nameof(GetAllBlogPostsUseCase), nameof(GetAllBlogPosts));
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "[{useCase}] - Error on try to execute {method}", nameof(GetAllBlogPostsUseCase), nameof(GetAllBlogPosts));
                throw;
            }

            return output;
        }
    }
}
