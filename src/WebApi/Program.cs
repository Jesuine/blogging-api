using Application.Interfaces;
using Application.UseCases.v1.CreateBlogPost;
using Application.UseCases.v1.CreateBlogPost.Models;
using Application.UseCases.v1.CreateBlogPost.Validators;
using Application.UseCases.v1.CreateComment;
using Application.UseCases.v1.CreateComment.Models;
using Application.UseCases.v1.CreateComment.Validators;
using Application.UseCases.v1.GetAllBlogPosts;
using Application.UseCases.v1.GetBlogPost;
using Application.UseCases.v1.GetBlogPost.Models;
using Application.UseCases.v1.GetBlogPost.Validators;
using FluentValidation;
using Infrastructure.Data.Connection;
using Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory>(
    _ => new SqlConnectionFactory(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<IGetAllBlogPostsUseCase, GetAllBlogPostsUseCase>();
builder.Services.AddScoped<ICreateBlogPostUseCase, CreateBlogPostUseCase>();
builder.Services.AddScoped<IGetBlogPostUseCase, GetBlogPostUseCase>();
builder.Services.AddScoped<ICreateCommentUseCase, CreateCommentUseCase>();

builder.Services.AddScoped<IValidator<CreateBlogPostsInput>, CreateBlogPostsInputValidator>();
builder.Services.AddScoped<IValidator<GetBlogPostInput>, GetBlogPostInputValidator>();
builder.Services.AddScoped<IValidator<CreateCommentInput>, CreateCommentInputValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
