using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.DTOs;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public sealed class AboutController(AboutPageService aboutPageService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await aboutPageService.GetAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UpdateAboutPageRequest request, CancellationToken cancellationToken)
    {
        var result = await aboutPageService.UpdateAsync(request.Content, cancellationToken);

        return Ok(result);
    }
}