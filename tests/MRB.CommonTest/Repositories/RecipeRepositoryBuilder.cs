using Moq;
using MRB.Domain.Repositories;

namespace MRB.CommonTest.Repositories;

public class RecipeRepositoryBuilder
{
    public static IRecipeRepository Build()
    {
        var mock = new Mock<IRecipeRepository>();
        return mock.Object;
    }
}