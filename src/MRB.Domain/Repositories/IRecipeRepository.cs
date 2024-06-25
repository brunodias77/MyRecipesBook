using MRB.Domain.Dtos;
using MRB.Domain.Entities;

namespace MRB.Domain.Repositories;

public interface IRecipeRepository
{
    Task Add(Recipe recipe);
    Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters);
    Task<IList<Recipe>> GetAll();
}