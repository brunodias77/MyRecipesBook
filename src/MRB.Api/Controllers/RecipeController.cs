using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MRB.Api.Binders;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.Application.UseCases.Recipes.GetById;
using MRB.Application.UseCases.Recipes.Register;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipeController : ControllerBase
{
    public RecipeController(IRecipeRepository recipeRepository, IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
    }

    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;

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

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter([FromServices] IFilterRecipeUseCase useCase,
        [FromBody] RequestFilterRecipeJson request)
    {
        var response = await useCase.Execute(request);
        if (response.Recipes.Any()) return Ok(response);
        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetRecipeByIdUseCase useCase,
        [FromRoute] [ModelBinder(typeof(MyRecipeBookRecipeIdBinder))] Guid id)
    {
        var response = await useCase.Execute(id);
        return Ok(null);
    }
}