using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Api.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<ShortUrl> ShortUrls { get; set; } = [];
}