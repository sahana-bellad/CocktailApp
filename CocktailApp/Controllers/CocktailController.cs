using CocktailApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CocktailApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CocktailController : ControllerBase
{
    private readonly ICocktailService _cocktailService;

    public CocktailController(ICocktailService cocktailService)
    {
        _cocktailService = cocktailService;
    }

    [HttpGet("random-cocktail")]
    public async Task<IActionResult> GetRandomCocktail([FromQuery] string language = "sith")
    {
        if (!string.Equals(language, "sith", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only 'sith' language is supported.");
        }

        var cocktail = await _cocktailService.GetRandomCocktail();
        if (cocktail == null)
            return NotFound("No cocktail found");

        return Ok(cocktail);
    }
}