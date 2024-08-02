using Bogus;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Domain.Enums;

namespace MRB.CommonTest.Requests.Recipes;

public class RequestFilterRecipeJsonBuilder
{
    public static RequestFilterRecipeJson Build()
    {
        return new Faker<RequestFilterRecipeJson>()
            .RuleFor(user => user.CookingTimes, faker => faker.Make(1, () => faker.PickRandom<CookingTime>()))
            .RuleFor(user => user.Difficulties, faker => faker.Make(1, () => faker.PickRandom<Difficulty>()))
            .RuleFor(user => user.DishTypes, faker => faker.Make(1, () => faker.PickRandom<DishType>()))
            .RuleFor(user => user.RecipeTitle_Ingredient, faker => faker.Lorem.Word());
    }
}