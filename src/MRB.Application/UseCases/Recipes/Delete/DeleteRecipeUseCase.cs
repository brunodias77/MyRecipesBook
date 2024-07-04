using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    public DeleteRecipeUseCase(ILoggedUser loggedUser, IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _recipeRepository = recipeRepository;
        _unitOfWork = unitOfWork;
    }

    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task Execute(Guid recipeId)
    {
        var loggedUser = await _loggedUser.User();
        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);
        if (recipe is null)
        {
            throw new NotFoundExecption(ResourceMessagesException.RECIPE_NOT_FOUND);
        }

        await _recipeRepository.Delete(recipeId);
        await _unitOfWork.Commit();
    }
}