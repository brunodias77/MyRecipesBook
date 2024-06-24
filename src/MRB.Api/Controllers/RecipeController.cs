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
    public async Task<IActionResult> Register(RequestRegisterRecipeJson request)
    {
        var recipe = new Recipe
        {
            Title = request.Title,
            CookingTime = request.CookingTime,
            Difficulty = request.Difficulty,
            UserId = Guid.NewGuid(), // Ajuste conforme necessário
            Ingredients = new List<Ingredient>(),
            Instructions = new List<Instruction>()
        };

        foreach (var ingredientName in request.Ingredients)
        {
            recipe.Ingredients.Add(new Ingredient
            {
                Item = ingredientName,
                RecipeId = recipe.Id
            });
        }

        foreach (var instructionRequest in request.Instructions)
        {
            recipe.Instructions.Add(new Instruction
            {
                Step = instructionRequest.Step,
                Text = instructionRequest.Text,
                RecipeId = recipe.Id
            });
        }

        // Adicione os tipos de pratos se necessário
        // foreach (var dishType in request.DishTypes)
        // {
        //     recipe.DishTypes.Add(dishType);
        // }

        await _recipeRepository.Add(recipe);

        return Ok();
    }

    // [HttpPost]
    // [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    // [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,
    //     [FromBody] RequestRegisterRecipeJson request)
    // {
    //     var response = await useCase.Execute(request);
    //     return Created(string.Empty, response);
    // }
}