using Moq;
using MRB.Domain.Dtos;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.CommonTest.Repositories;

public class RecipeRepositoryBuilder
{
    private readonly Mock<IRecipeRepository> _repository;

    public RecipeRepositoryBuilder()
    {
        _repository = new Mock<IRecipeRepository>();
    }

    public RecipeRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _repository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);
        return this;
    }

    public RecipeRepositoryBuilder GetById(User user, Recipe? recipe)
    {
        if (recipe is not null)
        {
            _repository.Setup(repo => repo.GetById(user, recipe.Id)).ReturnsAsync(recipe);
        }

        return this;
    }

    public IRecipeRepository Build()
    {
        return _repository.Object;
    }
}