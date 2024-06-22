using Microsoft.AspNetCore.Mvc;
using MRB.Application.UseCases.Recipes.Register;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Recipes;
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

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,
        [FromBody] RequestRegisterRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}