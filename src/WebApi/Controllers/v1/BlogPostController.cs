using Application.UseCases.v1.CreateBlogPost;
using Application.UseCases.v1.CreateBlogPost.Models;
using Application.UseCases.v1.CreateComment;
using Application.UseCases.v1.CreateComment.Models;
using Application.UseCases.v1.GetAllBlogPosts;
using Application.UseCases.v1.GetAllBlogPosts.Models;
using Application.UseCases.v1.GetBlogPost;
using Application.UseCases.v1.GetBlogPost.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Controllers.v1.Requests;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/posts")]
    public class BlogPostController : ControllerBase
    {
        private readonly ILogger<BlogPostController> _logger;
        private readonly IGetAllBlogPostsUseCase _getAllBlogPostsUseCase;
        private readonly ICreateBlogPostUseCase _createBlogPostUseCase;
        private readonly IGetBlogPostUseCase _getBlogPostUseCase;
        private readonly ICreateCommentUseCase _createCommentUseCase;

        public BlogPostController(
            ILogger<BlogPostController> logger,
            IGetAllBlogPostsUseCase getAllBlogPostsUseCase,
            ICreateBlogPostUseCase createBlogPostUseCase,
            IGetBlogPostUseCase getBlogPostUseCase,
            ICreateCommentUseCase createCommentUseCase)
        {
            _logger = logger;
            _getAllBlogPostsUseCase = getAllBlogPostsUseCase;
            _createBlogPostUseCase = createBlogPostUseCase;
            _getBlogPostUseCase = getBlogPostUseCase;
            _createCommentUseCase = createCommentUseCase;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllBlogPostsOutput))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBlogPostsAsync()
        {
            try
            {
                _logger.LogInformation("[{controller}] - Starting {method}", nameof(BlogPostController), nameof(GetAllBlogPostsAsync));

                GetAllBlogPostsOutput getAllBlogPostsOutput = await _getAllBlogPostsUseCase.GetAllBlogPosts();

                _logger.LogInformation("[{controller}] - Result: {@resul} - method {method}", nameof(BlogPostController), getAllBlogPostsOutput, nameof(GetAllBlogPostsAsync));

                return getAllBlogPostsOutput.IsValid ? Ok(getAllBlogPostsOutput) : NoContent();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "[{controller}] - Error {method}", nameof(BlogPostController), nameof(GetAllBlogPostsAsync));
                return StatusCode(statusCode: (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBlogPostsAsync([FromBody] BlogPostRequest blogPostRequest)
        {
            try
            {
                _logger.LogInformation("[{controller}] - Starting {method}", nameof(BlogPostController), nameof(CreateBlogPostsAsync));

                var input = new CreateBlogPostsInput(blogPostRequest.Title, blogPostRequest.Content, blogPostRequest.Id);

                CreateBlogPostOutput createBlogPostOutput = await _createBlogPostUseCase.CreateBlogPosts(input);

                _logger.LogInformation("[{controller}] - Result: {@resul} - method {method}", nameof(BlogPostController), createBlogPostOutput, nameof(CreateBlogPostsAsync));

                return createBlogPostOutput.IsValid ? StatusCode(statusCode: (int)HttpStatusCode.Created, createBlogPostOutput) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{controller}] - Error {method}", nameof(BlogPostController), nameof(CreateBlogPostsAsync));
                return StatusCode(statusCode: (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllBlogPostsOutput))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBlogPostByIdAsync([FromRoute] Guid id)
        {
            try
            {
                _logger.LogInformation("[{controller}] - Starting {method}", nameof(BlogPostController), nameof(GetBlogPostByIdAsync));

                Application.UseCases.v1.GetBlogPost.Models.GetBlogPostOutput getBlogPostOutput = await _getBlogPostUseCase.GetBlogPostById(new GetBlogPostInput(id));

                _logger.LogInformation("[{controller}] - Result: {@resul} - method {method}", nameof(BlogPostController), getBlogPostOutput, nameof(GetBlogPostByIdAsync));

                if (!getBlogPostOutput.IsValid)
                {
                    return getBlogPostOutput.Errors.Any(x => x.Contains("Invalid input")) ? BadRequest() : NoContent();
                }

                return Ok(getBlogPostOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{controller}] - Error {method}", nameof(BlogPostController), nameof(GetBlogPostByIdAsync));
                return StatusCode(statusCode: (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCommentAsync(
            [FromRoute] Guid id,
            [FromBody] CommentRequest commentRequest)
        {
            try
            {
                _logger.LogInformation("[{controller}] - Starting {method}", nameof(BlogPostController), nameof(CreateCommentAsync));

                var input = new CreateCommentInput(Id: commentRequest.Id, Content: commentRequest.Content, BlogPostId: id);

                CreateCommentOutput createCommentOutput = await _createCommentUseCase.CreateCommentAsync(input);

                _logger.LogInformation("[{controller}] - Result: {@resul} - method {method}", nameof(BlogPostController), createCommentOutput, nameof(CreateCommentAsync));

                return createCommentOutput.IsValid ? StatusCode(statusCode: (int)HttpStatusCode.Created, createCommentOutput) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{controller}] - Error {method}", nameof(BlogPostController), nameof(CreateCommentAsync));
                return StatusCode(statusCode: (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
