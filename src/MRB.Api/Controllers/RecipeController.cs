using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MRB.Application.UseCases.Recipes.Register;
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

    // [HttpPost]
    // public async Task<IActionResult> Register(RequestRegisterRecipeJson request)
    // {
    //     // var recipe = new Recipe
    //     // {
    //     //     Title = request.Title,
    //     //     CookingTime = (int?)request.CookingTime,
    //     //     Difficulty = (int?)request.Difficulty,
    //     //     UserId = Guid.Parse("850f33b4-8adc-47be-b0b1-3ec8d90b1cd2"), // Substitua pelo UserId real
    //     //     Ingredients = request.Ingredients.Select(i => new Ingredient { Item = i }).ToList(),
    //     //     Instructions = request.Instructions.Select(i => new Instruction { Step = i.Step, Text = i.Text }).ToList(),
    //     //     DishTypes = request.DishTypes.Select(d => new DishType { Type = (int)d }).ToList()
    //     // };
    //
    //     // var recipe = _mapper.Map<Recipe>(request);
    //     // recipe.UserId = Guid.Parse("850f33b4-8adc-47be-b0b1-3ec8d90b1cd2");
    //     //
    //     // await _recipeRepository.Add(recipe);
    //     //
    //     // return Ok(recipe);
    // }
}