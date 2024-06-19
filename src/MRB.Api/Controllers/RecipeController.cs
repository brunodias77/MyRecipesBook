using Microsoft.AspNetCore.Mvc;
using MRB.Domain.Repositories;

namespace MRB.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    public RecipeController(IRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    private readonly IRecipeRepository _recipeRepository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _recipeRepository.GetAll();
        return Ok(result);
    }
}