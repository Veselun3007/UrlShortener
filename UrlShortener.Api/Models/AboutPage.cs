namespace UrlShortener.Api.Models;

public class AboutPage
{
    public int Id { get; set; }

    public required string Content { get; set; }

    public DateTime UpdatedDate { get; set; }
}