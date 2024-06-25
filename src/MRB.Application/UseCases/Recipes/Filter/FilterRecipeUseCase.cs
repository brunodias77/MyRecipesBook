using AutoMapper;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Dtos;
using MRB.Domain.Enums;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    public FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeRepository recipeRepository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
    }

    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;

    public async Task<ResponseRecipeJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();
        var filters = new FilterRecipesDto
        {
            RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (CookingTime)c).ToList(),
            Difficulties = request.Difficulties.Distinct().Select(c => (Difficulty)c).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(c => (DishType)c).ToList()
        };
        var recipes = await _recipeRepository.Filter(loggedUser, filters);

        return new ResponseRecipeJson
        {
            Recipes = _mapper.Map<List<ResponseShortRecipeJson>>(recipes)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var validator = new FilterRecipeValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}