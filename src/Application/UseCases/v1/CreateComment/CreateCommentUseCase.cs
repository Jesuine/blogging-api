using Application.Interfaces;
using Application.UseCases.v1.CreateComment.Mappers;
using Application.UseCases.v1.CreateComment.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.v1.CreateComment
{
    public class CreateCommentUseCase : ICreateCommentUseCase
    {
        private ILogger<CreateCommentUseCase> _logger;
        private IPostRepository _postRepository;
        private IValidator<CreateCommentInput> _validator;
        public CreateCommentUseCase(
            ILogger<CreateCommentUseCase> logger,
            IPostRepository postRepository,
            IValidator<CreateCommentInput> validator)
        {
            _logger = logger;
            _postRepository = postRepository;
            _validator = validator;
        }

        public async Task<CreateCommentOutput> CreateCommentAsync(CreateCommentInput input)
        {
            var output = new CreateCommentOutput();
            try
            {
                _logger.LogInformation("[{useCase}] - Starting {method}", nameof(CreateCommentUseCase), nameof(CreateCommentAsync));

                var validationResult = await _validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("[{useCase}] - Invalid input for method {method}", nameof(CreateCommentUseCase), nameof(CreateCommentAsync));
                    output.AddError("Invalid input for create a comment");

                    return output;
                }

                output.Id = await _postRepository.AddCommentAsync(input.MapToComment());

                if (output.Id == Guid.Empty)
                {
                    _logger.LogError("[{useCase}] - Fail on insert Comment - method {method}", nameof(CreateCommentUseCase), nameof(CreateCommentAsync));
                    output.AddError("Fail on insert Comment");
                    return output;
                }

                _logger.LogInformation("[{useCase}] - Finishing {method}", nameof(CreateCommentUseCase), nameof(CreateCommentAsync));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{useCase}] - Error on try to execute {method}", nameof(CreateCommentUseCase), nameof(CreateCommentAsync));
                throw;
            }

            return output;
        }
    }
}