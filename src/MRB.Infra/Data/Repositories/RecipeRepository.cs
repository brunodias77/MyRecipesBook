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
        // return await _context.Recipes.Where(r => r.UserId == user.Id).ToListAsync();        // return await _context.Recipes

        // Inicia a query com receitas ativas e do usuário especificado
        var query = _context.Recipes.AsNoTracking()
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        // Inclui os ingredientes na query
        query = query.Include(r => r.Ingredients);

        query = query.Include(r => r.Instructions);

        // // Filtra por título da receita ou ingredientes, se especificado
        if (!string.IsNullOrWhiteSpace(filters.RecipeTitle_Ingredient))
        {
            query = query.Where(r => r.Title.Contains(filters.RecipeTitle_Ingredient) ||
                                     r.Ingredients.Any(i => i.Item.Contains(filters.RecipeTitle_Ingredient)));
        }

        // Filtra por dificuldades, se especificado
        if (filters.Difficulties != null && filters.Difficulties.Any())
        {
            query = query.Where(recipe => filters.Difficulties.Contains((Difficulty)recipe.Difficulty));
        }

        // // Retorna a lista de receitas filtradas
        return await query.ToListAsync();
    }
}