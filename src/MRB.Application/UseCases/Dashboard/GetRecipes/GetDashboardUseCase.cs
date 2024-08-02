using AutoMapper;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Repositories;
using MRB.Domain.Services;

namespace MRB.Application.UseCases.Dashboard.GetRecipes;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetDashboardUseCase(IRecipeRepository recipeRepository, IMapper mapper, ILoggedUser loggedUser)
    {
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }


    public async Task<ResponseRecipeJson> Execute()
    {
        var loggedUser = await _loggedUser.User();
        var recipes = await _recipeRepository.GetForDashboard(loggedUser);
        return new ResponseRecipeJson
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };
    }
}