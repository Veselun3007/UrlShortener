using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.DTOs;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public sealed class ShortUrlsController(ShortUrlService shortUrlService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await shortUrlService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await shortUrlService.GetByIdAsync(id, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateShortUrlRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;

        if (userId is null || userName is null)
        {
            return Unauthorized();
        }

        var result = await shortUrlService.CreateAsync(request.Url, userId, userName, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var isAdmin = User.IsInRole("Admin");

        await shortUrlService.DeleteAsync(id, userId, isAdmin, cancellationToken);

        return NoContent();
    }

    [HttpGet("~/s/{shortCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> Redirect(string shortCode, CancellationToken cancellationToken)
    {
        var originalUrl = await shortUrlService.GetOriginalUrlAsync(shortCode, cancellationToken);

        return Redirect(originalUrl);
    }
}