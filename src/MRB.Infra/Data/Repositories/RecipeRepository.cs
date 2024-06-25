using Microsoft.EntityFrameworkCore;
using MRB.Domain.Dtos;
using MRB.Domain.Entities;
using MRB.Domain.Enums;
using MRB.Domain.Extensions;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data.Repositories;

public class RecipeRepository : IRecipeRepository
{
    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<Recipe>> GetAll()
    {
        return await _context.Recipes
            .Include(r => r.Instructions)
            .Include(r => r.Ingredients)
            .Include(r => r.DishTypes)
            .ToListAsync();
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        // Inicia a query com receitas ativas e do usuário especificado
        var query = _context.Recipes.AsNoTracking().Where(recipe => recipe.Active && recipe.UserId == user.Id);

        // Filtra por dificuldades, se especificado
        if (filters.Difficulties.Any())
        {
            query = query.Where(recipe =>
                recipe.Difficulty.HasValue && filters.Difficulties.Contains((Difficulty)recipe.Difficulty.Value));
        }

        // Filtra por tempos de cozimento, se especificado
        if (filters.CookingTimes.Any())
        {
            query = query.Where(
                recipe => recipe.CookingTime.HasValue &&
                          filters.CookingTimes.Contains((CookingTime)recipe.CookingTime.Value));
        }

        // Filtra por tipos de pratos, se especificado
        if (filters.DishTypes.Any())
        {
            query = query.Where(
                recipe => recipe.DishTypes.Any(dishType =>
                    filters.DishTypes.Contains((Domain.Enums.DishType)dishType.Type)));
        }

        // Filtra por título ou ingrediente, se especificado
        if (filters.RecipeTitle_Ingredient.NotEmpty())
        {
            query = query.Where(recipe =>
                recipe.Title.Contains(filters.RecipeTitle_Ingredient) || recipe.Ingredients.Any(ingredient =>
                    ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));
        }

        // Executa a query e retorna a lista de receitas que atendem aos critérios de filtro
        return await query.ToListAsync();
    }
}