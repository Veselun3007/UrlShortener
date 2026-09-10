namespace UrlShortener.Api.Models;

public class ShortUrl 
{
    public Guid Id { get; set; }

    public required string OriginalUrl { get; set; }

    public required string ShortCode { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedById { get; set; } 

    public ApplicationUser CreatedBy { get; set; } 
}