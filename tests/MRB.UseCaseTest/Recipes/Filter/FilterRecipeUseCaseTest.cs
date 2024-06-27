using FluentAssertions;
using MRB.Application.UseCases.Recipes.Filter;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests.Recipes;
using MRB.Domain.Entities;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Filter;

public class FilterRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestFilterRecipeJsonBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(user, recipes);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Recipes.Should().NotBeNullOrEmpty();
        result.Recipes.Should().HaveCount(recipes.Count);
    }

    private static FilterRecipeUseCase CreateUseCase(User user, IList<Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeRepository = new RecipeRepositoryBuilder().Filter(user, recipes).Build();
        return new FilterRecipeUseCase(mapper, loggedUser, recipeRepository);
    }
}