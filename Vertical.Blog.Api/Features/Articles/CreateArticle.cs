namespace Vertical.Blog.Api.Features.Articles;

public static class CreateArticle
{
    public class Command
    { 
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
    }
}
