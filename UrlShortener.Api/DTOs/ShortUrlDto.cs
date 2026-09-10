namespace UrlShortener.Api.DTOs;

public class ShortUrlDto
{
    public Guid Id { get; set; }

    public required string OriginalUrl { get; set; }

    public required string ShortCode { get; set; }

    public DateTime CreatedDate { get; set; }

    public required string CreatedBy { get; set; }
}