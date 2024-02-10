using Carter;
using MediatR;
using Vertical.Blog.Api.Database;
using Vertical.Blog.Api.Entities;

namespace Vertical.Blog.Api.Features.Articles;

public static class CreateArticle
{
    public class Command : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
    }

    internal sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Tags = request.Tags,
                CreatedOnUtc = DateTime.UtcNow,
            };

            _dbContext.Add(article);
            await _dbContext.SaveChangesAsync();

            return article.Id;
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/articles", async (Command command, ISender sender) =>
            {
                var articleId = await sender.Send(command);
                return Results.Ok(articleId);
            });
        }
    }

}
