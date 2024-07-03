using AutoMapper;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    public GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeRepository recipeRepository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
    }

    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;

    public async Task<ResponseRecipeJson> Execute(Guid recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);

        if (recipe is null) throw new NotFoundExecption(ResourceMessagesException.RECIPE_NOT_FOUND);
        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}